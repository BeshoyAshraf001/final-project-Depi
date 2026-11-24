using Microsoft.EntityFrameworkCore;
using MobileAdvisor.DAL.Data;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.DAL.Repositories.Implementations;

public class ExpertProfileRepository : GenericRepository<ExpertProfile>, IExpertProfileRepository
{
    public ExpertProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExpertProfile>> GetApprovedExpertsAsync(int? specialtyId = null)
    {
        var query = _context.ExpertProfiles
            .Include(e => e.User)
            .Include(e => e.ExpertSpecialties)
                .ThenInclude(es => es.Specialty)
            .Where(e => e.IsApproved && e.IsAvailable);

        if (specialtyId.HasValue)
        {
            query = query.Where(e => e.ExpertSpecialties.Any(es => es.SpecialtyId == specialtyId.Value));
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<ExpertProfile?> GetByUserIdAsync(int userId)
    {
        return await _context.ExpertProfiles
            .Include(e => e.User)
            .Include(e => e.ExpertSpecialties)
                .ThenInclude(es => es.Specialty)
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }
}
