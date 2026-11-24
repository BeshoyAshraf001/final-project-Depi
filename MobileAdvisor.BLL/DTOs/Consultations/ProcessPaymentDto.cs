namespace MobileAdvisor.BLL.DTOs.Consultations;

public record ProcessPaymentDto(
    bool IsSuccessful,
    string? TransactionReference
);
