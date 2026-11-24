using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class Phone : BaseAuditableEntity
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public decimal GuidePrice { get; set; }
    public string? Specifications { get; set; }
    public bool IsActive { get; set; } = true;
}
