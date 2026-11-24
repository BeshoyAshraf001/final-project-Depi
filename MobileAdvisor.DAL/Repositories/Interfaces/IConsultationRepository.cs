using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Repositories.Interfaces;

public interface IConsultationRepository : IGenericRepository<Consultation>
{
    Task<Consultation?> GetConsultationWithDetailsAsync(int id);
    Task<IEnumerable<Consultation>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Consultation>> GetByExpertIdAsync(int expertId);
}
