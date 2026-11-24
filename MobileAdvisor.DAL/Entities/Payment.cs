using MobileAdvisor.DAL.Entities.Common;
using MobileAdvisor.DAL.Entities.Enums;

namespace MobileAdvisor.DAL.Entities;

public class Payment : BaseEntity
{
    public int ConsultationId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? TransactionReference { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    public Consultation Consultation { get; set; } = null!;
}
