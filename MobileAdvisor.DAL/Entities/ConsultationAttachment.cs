using MobileAdvisor.DAL.Entities.Common;

namespace MobileAdvisor.DAL.Entities;

public class ConsultationAttachment : BaseEntity
{
    public int ConsultationId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Consultation Consultation { get; set; } = null!;
}
