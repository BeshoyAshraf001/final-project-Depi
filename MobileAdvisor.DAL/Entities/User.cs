using MobileAdvisor.DAL.Entities.Common;
using MobileAdvisor.DAL.Entities.Enums;

namespace MobileAdvisor.DAL.Entities;

public class User : BaseAuditableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public AccountStatus Status { get; set; } = AccountStatus.Active;

    // Navigation Properties
    public ExpertProfile? ExpertProfile { get; set; }
    public ICollection<Consultation> CustomerConsultations { get; set; } = new List<Consultation>();
    public ICollection<Consultation> ExpertConsultations { get; set; } = new List<Consultation>();
    public ICollection<Review> GivenReviews { get; set; } = new List<Review>();
    public ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
