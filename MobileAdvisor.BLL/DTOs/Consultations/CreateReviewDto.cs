namespace MobileAdvisor.BLL.DTOs.Consultations;

public record CreateReviewDto(
    int Rating,
    string? Comment
);
