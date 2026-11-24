namespace MobileAdvisor.BLL.DTOs.Consultations;

public record CreateNewPhoneConsultationDto(
    int ExpertId,
    string Description,
    decimal Budget,
    string PrioritiesAndUsage
);
