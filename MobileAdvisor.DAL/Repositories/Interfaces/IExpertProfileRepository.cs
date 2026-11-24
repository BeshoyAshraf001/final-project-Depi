using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Repositories.Interfaces;

public interface IExpertProfileRepository : IGenericRepository<ExpertProfile>
{
    Task<IEnumerable<ExpertProfile>> GetApprovedExpertsAsync(int? specialtyId = null);
    Task<ExpertProfile?> GetByUserIdAsync(int userId);
}
