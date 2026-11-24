using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class ExpertRecommendation : BaseAuditableEntity
{
    public int ConsultationId { get; set; }
    public string RecommendationText { get; set; } = string.Empty;
    public string? InitialValuation { get; set; }
    public string? InspectionChecklist { get; set; } // نقاط ينصح بفحصها

    public Consultation Consultation { get; set; } = null!;
}
