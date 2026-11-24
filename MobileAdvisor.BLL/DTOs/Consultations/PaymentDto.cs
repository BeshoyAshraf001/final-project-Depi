namespace MobileAdvisor.BLL.DTOs.Consultations;

public record PaymentDto(
    decimal Amount,
    string Status,
    string? TransactionReference,
    DateTime ProcessedAt
);
