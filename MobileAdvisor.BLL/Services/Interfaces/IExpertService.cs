using MobileAdvisor.BLL.DTOs;

namespace MobileAdvisor.BLL.Services.Interfaces;

public interface IExpertService
{
    Task<IEnumerable<ExpertProfileDto>> GetApprovedExpertsAsync(int? specialtyId = null);
    Task<ExpertProfileDto?> GetProfileByUserIdAsync(int userId);
    Task<ExpertProfileDto> UpdateProfileAsync(int userId, UpdateExpertProfileDto dto);
    Task ApproveExpertAsync(int expertProfileId);
}
