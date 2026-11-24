namespace MobileAdvisor.BLL.DTOs.Experts;

public record UpdateExpertProfileDto(
    string Bio,
    int YearsOfExperience,
    decimal DefaultFee,
    bool IsAvailable,
    List<int> SpecialtyIds
);
