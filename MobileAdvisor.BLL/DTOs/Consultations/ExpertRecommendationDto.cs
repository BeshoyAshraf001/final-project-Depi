namespace MobileAdvisor.BLL.DTOs.Consultations;

public record ExpertRecommendationDto(
    string RecommendationText,
    string? InitialValuation,
    string? InspectionChecklist,
    DateTime CreatedAt
);
