using MobileAdvisor.BLL.DTOs.Phones;
using MobileAdvisor.BLL.Services.Interfaces;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.BLL.Services.Implementations;

public class PhoneService : IPhoneService
{
    private readonly IUnitOfWork _unitOfWork;

    public PhoneService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PhoneDto>> GetAllPhonesAsync()
    {
        var phones = await _unitOfWork.Phones.GetAllAsync();
        return phones.Select(p => new PhoneDto(p.Id, p.Brand, p.Model, p.GuidePrice, p.Specifications, p.IsActive));
    }

    public async Task<IEnumerable<PhoneDto>> RecommendPhonesAsync(decimal budget, string? keyword = null)
    {
        var phones = await _unitOfWork.Phones.FindAsync(p => p.IsActive && p.GuidePrice <= budget);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            phones = phones.Where(p => 
                p.Brand.Contains(keyword, StringComparison.OrdinalIgnoreCase) || 
                p.Model.Contains(keyword, StringComparison.OrdinalIgnoreCase) || 
                (p.Specifications != null && p.Specifications.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );
        }

        return phones.OrderByDescending(p => p.GuidePrice)
            .Select(p => new PhoneDto(p.Id, p.Brand, p.Model, p.GuidePrice, p.Specifications, p.IsActive));
    }

    public async Task<PhoneDto> CreatePhoneAsync(CreateOrUpdatePhoneDto dto)
    {
        var phone = new Phone
        {
            Brand = dto.Brand,
            Model = dto.Model,
            GuidePrice = dto.GuidePrice,
            Specifications = dto.Specifications,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Phones.AddAsync(phone);
        await _unitOfWork.CompleteAsync();

        return new PhoneDto(phone.Id, phone.Brand, phone.Model, phone.GuidePrice, phone.Specifications, phone.IsActive);
    }

    public async Task UpdatePhoneAsync(int id, CreateOrUpdatePhoneDto dto)
    {
        var phone = await _unitOfWork.Phones.GetByIdAsync(id);
        if (phone == null) throw new KeyNotFoundException("Phone not found.");

        phone.Brand = dto.Brand;
        phone.Model = dto.Model;
        phone.GuidePrice = dto.GuidePrice;
        phone.Specifications = dto.Specifications;
        phone.IsActive = dto.IsActive;

        _unitOfWork.Phones.Update(phone);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeletePhoneAsync(int id)
    {
        var phone = await _unitOfWork.Phones.GetByIdAsync(id);
        if (phone == null) throw new KeyNotFoundException("Phone not found.");

        _unitOfWork.Phones.Delete(phone);
        await _unitOfWork.CompleteAsync();
    }
}
