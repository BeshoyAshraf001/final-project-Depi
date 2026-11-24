namespace MobileAdvisor.BLL.DTOs.Phones;

public record CreateOrUpdatePhoneDto(
    string Brand,
    string Model,
    decimal GuidePrice,
    string? Specifications,
    bool IsActive
);
