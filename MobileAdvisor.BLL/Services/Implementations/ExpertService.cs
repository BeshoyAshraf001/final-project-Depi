using MobileAdvisor.BLL.DTOs.Experts;
using MobileAdvisor.BLL.Services.Interfaces;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Entities.Enums;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.BLL.Services.Implementations;

public class ExpertService : IExpertService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExpertService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ExpertProfileDto>> GetApprovedExpertsAsync(int? specialtyId = null)
    {
        var experts = await _unitOfWork.ExpertProfiles.GetApprovedExpertsAsync(specialtyId);
        return experts.Select(e => new ExpertProfileDto(
            e.Id,
            e.UserId,
            e.User.FullName,
            e.User.Email,
            e.Bio,
            e.YearsOfExperience,
            e.DefaultFee,
            e.IsAvailable,
            e.IsApproved,
            e.ExpertSpecialties.Select(es => es.Specialty.Name).ToList()
        ));
    }

    public async Task<ExpertProfileDto?> GetProfileByUserIdAsync(int userId)
    {
        var e = await _unitOfWork.ExpertProfiles.GetByUserIdAsync(userId);
        if (e == null) return null;

        return new ExpertProfileDto(
            e.Id,
            e.UserId,
            e.User.FullName,
            e.User.Email,
            e.Bio,
            e.YearsOfExperience,
            e.DefaultFee,
            e.IsAvailable,
            e.IsApproved,
            e.ExpertSpecialties.Select(es => es.Specialty.Name).ToList()
        );
    }

    public async Task<ExpertProfileDto> UpdateProfileAsync(int userId, UpdateExpertProfileDto dto)
    {
        if (dto.DefaultFee < 0)
        {
            throw new ArgumentException("Default fee cannot be negative.");
        }

        var profile = await _unitOfWork.ExpertProfiles.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new KeyNotFoundException("Expert profile not found.");
        }

        profile.Bio = dto.Bio;
        profile.YearsOfExperience = dto.YearsOfExperience;
        profile.DefaultFee = dto.DefaultFee;
        profile.IsAvailable = dto.IsAvailable;

        // Clear existing specialties and re-assign
        var existingSpecs = await _unitOfWork.ExpertSpecialties.FindAsync(es => es.ExpertProfileId == profile.Id);
        foreach (var s in existingSpecs)
        {
            _unitOfWork.ExpertSpecialties.Delete(s);
        }

        foreach (var specId in dto.SpecialtyIds)
        {
            await _unitOfWork.ExpertSpecialties.AddAsync(new ExpertSpecialty
            {
                ExpertProfileId = profile.Id,
                SpecialtyId = specId
            });
        }

        _unitOfWork.ExpertProfiles.Update(profile);
        await _unitOfWork.CompleteAsync();

        return (await GetProfileByUserIdAsync(userId))!;
    }

    public async Task ApproveExpertAsync(int expertProfileId)
    {
        var profile = await _unitOfWork.ExpertProfiles.GetByIdAsync(expertProfileId);
        if (profile == null) throw new KeyNotFoundException("Expert profile not found.");

        profile.IsApproved = true;
        profile.ApprovedAt = DateTime.UtcNow;
        _unitOfWork.ExpertProfiles.Update(profile);

        var user = await _unitOfWork.Users.GetByIdAsync(profile.UserId);
        if (user != null)
        {
            user.Status = AccountStatus.Active;
            _unitOfWork.Users.Update(user);
        }

        await _unitOfWork.CompleteAsync();
    }
}
