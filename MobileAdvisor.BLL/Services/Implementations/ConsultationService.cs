using MobileAdvisor.BLL.DTOs.Consultations;
using MobileAdvisor.BLL.DTOs.Messages;
using MobileAdvisor.BLL.Services.Interfaces;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Entities.Enums;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.BLL.Services.Implementations;

public class ConsultationService : IConsultationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ConsultationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ConsultationResponseDto> CreateNewPhoneConsultationAsync(int customerId, CreateNewPhoneConsultationDto dto)
    {
        var expert = await _unitOfWork.ExpertProfiles.GetByUserIdAsync(dto.ExpertId);
        if (expert == null || !expert.IsApproved)
        {
            throw new ArgumentException("Expert does not exist or is not approved.");
        }

        var consultation = new Consultation
        {
            CustomerId = customerId,
            ExpertId = dto.ExpertId,
            Type = ConsultationType.NewPhoneRecommendation,
            Status = ConsultationStatus.PendingExpertReview,
            Description = dto.Description,
            Budget = dto.Budget,
            PrioritiesAndUsage = dto.PrioritiesAndUsage,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Consultations.AddAsync(consultation);
        await _unitOfWork.CompleteAsync();

        return (await GetByIdAsync(consultation.Id, customerId, UserRole.Customer.ToString()))!;
    }

    public async Task<ConsultationResponseDto> CreateUsedPhoneConsultationAsync(
        int customerId,
        CreateUsedPhoneConsultationDto dto,
        List<(string FileName, Stream Stream, string ContentType)>? files = null)
    {
        var expert = await _unitOfWork.ExpertProfiles.GetByUserIdAsync(dto.ExpertId);
        if (expert == null || !expert.IsApproved)
        {
            throw new ArgumentException("Expert does not exist or is not approved.");
        }

        var consultation = new Consultation
        {
            CustomerId = customerId,
            ExpertId = dto.ExpertId,
            Type = ConsultationType.UsedPhoneEvaluation,
            Status = ConsultationStatus.PendingExpertReview,
            Description = dto.Description,
            UsedPhoneModel = dto.UsedPhoneModel,
            UsedPhoneAskingPrice = dto.UsedPhoneAskingPrice,
            UsedPhoneConditionNotes = dto.UsedPhoneConditionNotes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Consultations.AddAsync(consultation);
        await _unitOfWork.CompleteAsync();

        // Handle attachments if provided (1 to 5 images as per FR-09)
        if (files != null && files.Any())
        {
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", consultation.Id.ToString());
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            foreach (var (fileName, stream, contentType) in files.Take(5))
            {
                var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
                var fullPath = Path.Combine(uploadDir, uniqueName);
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fs);
                }

                await _unitOfWork.ConsultationAttachments.AddAsync(new ConsultationAttachment
                {
                    ConsultationId = consultation.Id,
                    FileName = fileName,
                    FilePath = $"/uploads/{consultation.Id}/{uniqueName}",
                    ContentType = contentType,
                    FileSizeBytes = stream.Length,
                    UploadedAt = DateTime.UtcNow
                });
            }
            await _unitOfWork.CompleteAsync();
        }

        return (await GetByIdAsync(consultation.Id, customerId, UserRole.Customer.ToString()))!;
    }

    public async Task<ConsultationResponseDto> GetByIdAsync(int consultationId, int currentUserId, string role)
    {
        var c = await _unitOfWork.Consultations.GetConsultationWithDetailsAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");

        // BR-05 & NFR-02: Permission check
        if (role != UserRole.Admin.ToString() && c.CustomerId != currentUserId && c.ExpertId != currentUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to view this consultation.");
        }

        return MapToDto(c);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetMyConsultationsAsync(int userId, string role)
    {
        IEnumerable<Consultation> list;
        if (role == UserRole.Customer.ToString())
        {
            list = await _unitOfWork.Consultations.GetByCustomerIdAsync(userId);
        }
        else if (role == UserRole.Expert.ToString())
        {
            list = await _unitOfWork.Consultations.GetByExpertIdAsync(userId);
        }
        else
        {
            list = await _unitOfWork.Consultations.GetAllAsync();
        }

        return list.Select(MapToDto);
    }

    public async Task ExpertReviewOrderAsync(int consultationId, int expertId, bool accept, decimal? agreedPrice)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.ExpertId != expertId) throw new UnauthorizedAccessException("Not assigned to this consultation.");
        if (c.Status != ConsultationStatus.PendingExpertReview) throw new InvalidOperationException("Invalid state transition.");

        if (accept)
        {
            c.Status = ConsultationStatus.AwaitingCustomerApproval;
            c.AgreedPrice = agreedPrice ?? 0;
        }
        else
        {
            c.Status = ConsultationStatus.Rejected;
        }

        c.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Consultations.Update(c);
        await _unitOfWork.CompleteAsync();
    }

    public async Task CustomerApprovePriceAsync(int consultationId, int customerId, bool accept)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.CustomerId != customerId) throw new UnauthorizedAccessException("Not customer of this consultation.");
        if (c.Status != ConsultationStatus.AwaitingCustomerApproval) throw new InvalidOperationException("Invalid state transition.");

        if (accept)
        {
            // If free (price = 0), straight to ReadyToStart, otherwise AwaitingPayment
            c.Status = (c.AgreedPrice == null || c.AgreedPrice == 0)
                ? ConsultationStatus.ReadyToStart
                : ConsultationStatus.AwaitingPayment;
        }
        else
        {
            c.Status = ConsultationStatus.Cancelled;
        }

        c.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Consultations.Update(c);
        await _unitOfWork.CompleteAsync();
    }

    public async Task ProcessTestPaymentAsync(int consultationId, int customerId, ProcessPaymentDto dto)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.CustomerId != customerId) throw new UnauthorizedAccessException("Not authorized.");
        if (c.Status != ConsultationStatus.AwaitingPayment) throw new InvalidOperationException("Consultation not awaiting payment.");

        var payment = new Payment
        {
            ConsultationId = c.Id,
            Amount = c.AgreedPrice ?? 0,
            Status = dto.IsSuccessful ? PaymentStatus.Success : PaymentStatus.Failed,
            TransactionReference = dto.TransactionReference ?? Guid.NewGuid().ToString("N"),
            ProcessedAt = DateTime.UtcNow
        };

        await _unitOfWork.Payments.AddAsync(payment);

        if (dto.IsSuccessful)
        {
            c.Status = ConsultationStatus.ReadyToStart;
        }
        else
        {
            c.Status = ConsultationStatus.PaymentFailed; // BR-04
        }

        c.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Consultations.Update(c);
        await _unitOfWork.CompleteAsync();
    }

    public async Task AddRecommendationAsync(int consultationId, int expertId, CreateRecommendationDto dto)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.ExpertId != expertId) throw new UnauthorizedAccessException("Not assigned to this consultation.");

        var recommendation = new ExpertRecommendation
        {
            ConsultationId = c.Id,
            RecommendationText = dto.RecommendationText,
            InitialValuation = dto.InitialValuation,
            InspectionChecklist = dto.InspectionChecklist,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ExpertRecommendations.AddAsync(recommendation);
        c.Status = ConsultationStatus.InProgress;
        c.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Consultations.Update(c);
        await _unitOfWork.CompleteAsync();
    }

    public async Task CompleteConsultationAsync(int consultationId, int expertId)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.ExpertId != expertId) throw new UnauthorizedAccessException("Not authorized.");

        c.Status = ConsultationStatus.Completed;
        c.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Consultations.Update(c);
        await _unitOfWork.CompleteAsync();
    }

    public async Task AddReviewAsync(int consultationId, int customerId, CreateReviewDto dto)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.CustomerId != customerId) throw new UnauthorizedAccessException("Not authorized.");
        if (c.Status != ConsultationStatus.Completed) throw new InvalidOperationException("Can only review completed consultations."); // BR-02
        if (dto.Rating < 1 || dto.Rating > 5) throw new ArgumentException("Rating must be between 1 and 5."); // BR-06

        if (await _unitOfWork.Reviews.AnyAsync(r => r.ConsultationId == consultationId))
        {
            throw new InvalidOperationException("Review already submitted.");
        }

        var review = new Review
        {
            ConsultationId = c.Id,
            CustomerId = customerId,
            ExpertId = c.ExpertId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<MessageDto> SendMessageAsync(int consultationId, int senderId, string content)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.CustomerId != senderId && c.ExpertId != senderId)
        {
            throw new UnauthorizedAccessException("User not participant of this consultation.");
        }

        // BR-09: Must be in ReadyToStart, InProgress, or Completed
        if (c.Status != ConsultationStatus.ReadyToStart && c.Status != ConsultationStatus.InProgress && c.Status != ConsultationStatus.Completed)
        {
            throw new InvalidOperationException("Cannot send messages until consultation is accepted and ready.");
        }

        if (c.Status == ConsultationStatus.ReadyToStart)
        {
            c.Status = ConsultationStatus.InProgress;
            _unitOfWork.Consultations.Update(c);
        }

        var sender = await _unitOfWork.Users.GetByIdAsync(senderId);

        var msg = new Message
        {
            ConsultationId = consultationId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        await _unitOfWork.Messages.AddAsync(msg);
        await _unitOfWork.CompleteAsync();

        return new MessageDto(msg.Id, msg.ConsultationId, msg.SenderId, sender?.FullName ?? "Unknown", msg.Content, msg.SentAt);
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int consultationId, int currentUserId)
    {
        var c = await _unitOfWork.Consultations.GetByIdAsync(consultationId);
        if (c == null) throw new KeyNotFoundException("Consultation not found.");
        if (c.CustomerId != currentUserId && c.ExpertId != currentUserId)
        {
            throw new UnauthorizedAccessException("Not authorized.");
        }

        var messages = await _unitOfWork.Messages.FindAsync(m => m.ConsultationId == consultationId);
        var users = (await _unitOfWork.Users.GetAllAsync()).ToDictionary(u => u.Id, u => u.FullName);

        return messages.OrderBy(m => m.SentAt).Select(m => new MessageDto(
            m.Id,
            m.ConsultationId,
            m.SenderId,
            users.TryGetValue(m.SenderId, out var name) ? name : "Unknown",
            m.Content,
            m.SentAt
        ));
    }

    private static ConsultationResponseDto MapToDto(Consultation c)
    {
        return new ConsultationResponseDto(
            c.Id,
            c.CustomerId,
            c.Customer?.FullName ?? string.Empty,
            c.ExpertId,
            c.Expert?.FullName ?? string.Empty,
            c.Type.ToString(),
            c.Status.ToString(),
            c.Description,
            c.Budget,
            c.PrioritiesAndUsage,
            c.UsedPhoneModel,
            c.UsedPhoneAskingPrice,
            c.UsedPhoneConditionNotes,
            c.AgreedPrice,
            c.CreatedAt,
            c.Attachments?.Select(a => a.FilePath).ToList() ?? new List<string>(),
            c.Recommendation != null ? new ExpertRecommendationDto(
                c.Recommendation.RecommendationText,
                c.Recommendation.InitialValuation,
                c.Recommendation.InspectionChecklist,
                c.Recommendation.CreatedAt
            ) : null,
            c.Payment != null ? new PaymentDto(
                c.Payment.Amount,
                c.Payment.Status.ToString(),
                c.Payment.TransactionReference,
                c.Payment.ProcessedAt
            ) : null,
            c.Review != null ? new ReviewDto(
                c.Review.Rating,
                c.Review.Comment,
                c.Review.CreatedAt
            ) : null
        );
    }
}
