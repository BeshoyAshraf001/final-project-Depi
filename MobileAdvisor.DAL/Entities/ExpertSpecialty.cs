namespace MobileAdvisor.DAL.Entities;

public class ExpertSpecialty
{
    public int ExpertProfileId { get; set; }
    public ExpertProfile ExpertProfile { get; set; } = null!;

    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}
