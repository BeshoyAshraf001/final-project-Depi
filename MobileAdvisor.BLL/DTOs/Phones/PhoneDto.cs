namespace MobileAdvisor.BLL.DTOs.Phones;

public record PhoneDto(
    int Id,
    string Brand,
    string Model,
    decimal GuidePrice,
    string? Specifications,
    bool IsActive
);
