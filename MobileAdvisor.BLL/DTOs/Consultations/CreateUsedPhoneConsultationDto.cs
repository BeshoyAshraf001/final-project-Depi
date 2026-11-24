namespace MobileAdvisor.BLL.DTOs.Consultations;

public record CreateUsedPhoneConsultationDto(
    int ExpertId,
    string Description,
    string UsedPhoneModel,
    decimal UsedPhoneAskingPrice,
    string UsedPhoneConditionNotes
);
