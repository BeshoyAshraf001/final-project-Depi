using MobileAdvisor.DAL.Entities.Common;
using MobileAdvisor.DAL.Entities.Enums;

namespace MobileAdvisor.DAL.Entities;

public class Consultation : BaseAuditableEntity
{
    public int CustomerId { get; set; }
    public int ExpertId { get; set; }
    public ConsultationType Type { get; set; }
    public ConsultationStatus Status { get; set; } = ConsultationStatus.PendingExpertReview;

    // Common fields
    public string Description { get; set; } = string.Empty;

    // Fields for New Phone Recommendation (FR-07)
    public decimal? Budget { get; set; }
    public string? PrioritiesAndUsage { get; set; }

    // Fields for Used Phone Evaluation (FR-08)
    public string? UsedPhoneModel { get; set; }
    public decimal? UsedPhoneAskingPrice { get; set; }
    public string? UsedPhoneConditionNotes { get; set; }

    // Financial Agreement (FR-13, FR-14, BR-03)
    public decimal? AgreedPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public User Customer { get; set; } = null!;
    public User Expert { get; set; } = null!;
    public ICollection<ConsultationAttachment> Attachments { get; set; } = new List<ConsultationAttachment>();
    public ExpertRecommendation? Recommendation { get; set; }
    public Payment? Payment { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public Review? Review { get; set; }
}
