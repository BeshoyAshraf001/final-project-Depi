namespace MobileAdvisor.BLL.DTOs.Experts;

public record ExpertProfileDto(
    int Id,
    int UserId,
    string FullName,
    string Email,
    string Bio,
    int YearsOfExperience,
    decimal DefaultFee,
    bool IsAvailable,
    bool IsApproved,
    List<string> Specialties
);
