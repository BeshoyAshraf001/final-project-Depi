namespace MobileAdvisor.BLL.DTOs.Consultations;

public record ConsultationResponseDto(
    int Id,
    int CustomerId,
    string CustomerName,
    int ExpertId,
    string ExpertName,
    string Type,
    string Status,
    string Description,
    decimal? Budget,
    string? PrioritiesAndUsage,
    string? UsedPhoneModel,
    decimal? UsedPhoneAskingPrice,
    string? UsedPhoneConditionNotes,
    decimal? AgreedPrice,
    DateTime CreatedAt,
    List<string> AttachmentUrls,
    ExpertRecommendationDto? Recommendation,
    PaymentDto? Payment,
    ReviewDto? Review
);
