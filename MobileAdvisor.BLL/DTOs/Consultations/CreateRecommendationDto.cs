namespace MobileAdvisor.BLL.DTOs.Consultations;

public record CreateRecommendationDto(
    string RecommendationText,
    string? InitialValuation,
    string? InspectionChecklist
);
