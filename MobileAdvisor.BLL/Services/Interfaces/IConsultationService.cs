using MobileAdvisor.BLL.DTOs;

namespace MobileAdvisor.BLL.Services.Interfaces;

public interface IConsultationService
{
    Task<ConsultationResponseDto> CreateNewPhoneConsultationAsync(int customerId, CreateNewPhoneConsultationDto dto);
    Task<ConsultationResponseDto> CreateUsedPhoneConsultationAsync(int customerId, CreateUsedPhoneConsultationDto dto, List<(string FileName, Stream Stream, string ContentType)>? files = null);
    Task<ConsultationResponseDto> GetByIdAsync(int consultationId, int currentUserId, string role);
    Task<IEnumerable<ConsultationResponseDto>> GetMyConsultationsAsync(int userId, string role);
    Task ExpertReviewOrderAsync(int consultationId, int expertId, bool accept, decimal? agreedPrice);
    Task CustomerApprovePriceAsync(int consultationId, int customerId, bool accept);
    Task ProcessTestPaymentAsync(int consultationId, int customerId, ProcessPaymentDto dto);
    Task AddRecommendationAsync(int consultationId, int expertId, CreateRecommendationDto dto);
    Task CompleteConsultationAsync(int consultationId, int expertId);
    Task AddReviewAsync(int consultationId, int customerId, CreateReviewDto dto);
    Task<MessageDto> SendMessageAsync(int consultationId, int senderId, string content);
    Task<IEnumerable<MessageDto>> GetMessagesAsync(int consultationId, int currentUserId);
}
