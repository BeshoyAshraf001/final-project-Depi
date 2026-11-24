using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class ExpertProfile : BaseEntity
{
    public int UserId { get; set; }
    public string Bio { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public decimal DefaultFee { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<ExpertSpecialty> ExpertSpecialties { get; set; } = new List<ExpertSpecialty>();
}
