using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class Message : BaseEntity
{
    public int ConsultationId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Consultation Consultation { get; set; } = null!;
    public User Sender { get; set; } = null!;
}
