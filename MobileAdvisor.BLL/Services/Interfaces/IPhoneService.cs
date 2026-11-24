using MobileAdvisor.BLL.DTOs;

namespace MobileAdvisor.BLL.Services.Interfaces;

public interface IPhoneService
{
    Task<IEnumerable<PhoneDto>> GetAllPhonesAsync();
    Task<IEnumerable<PhoneDto>> RecommendPhonesAsync(decimal budget, string? keyword = null);
    Task<PhoneDto> CreatePhoneAsync(CreateOrUpdatePhoneDto dto);
    Task UpdatePhoneAsync(int id, CreateOrUpdatePhoneDto dto);
    Task DeletePhoneAsync(int id);
}
