using MobileAdvisor.DAL.Entities.Enums;

namespace MobileAdvisor.BLL.DTOs.Auth;

public record RegisterRequestDto(string FullName, string Email, string Password, string? PhoneNumber, UserRole Role);
