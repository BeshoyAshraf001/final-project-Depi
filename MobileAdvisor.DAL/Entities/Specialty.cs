using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class Specialty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<ExpertSpecialty> ExpertSpecialties { get; set; } = new List<ExpertSpecialty>();
}
