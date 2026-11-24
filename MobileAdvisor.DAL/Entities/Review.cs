using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class Review : BaseAuditableEntity
{
    public int ConsultationId { get; set; }
    public int CustomerId { get; set; }
    public int ExpertId { get; set; }
    public int Rating { get; set; } // 1 to 5
    public string? Comment { get; set; }

    public Consultation Consultation { get; set; } = null!;
    public User Customer { get; set; } = null!;
    public User Expert { get; set; } = null!;
}
