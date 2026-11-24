namespace MobileAdvisor.BLL.DTOs.Consultations;

public record ReviewDto(
    int Rating,
    string? Comment,
    DateTime CreatedAt
);
