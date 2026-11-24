namespace MobileAdvisor.BLL.DTOs.Messages;

public record MessageDto(
    int Id,
    int ConsultationId,
    int SenderId,
    string SenderName,
    string Content,
    DateTime SentAt
);
