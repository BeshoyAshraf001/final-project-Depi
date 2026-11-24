using Microsoft.EntityFrameworkCore;
using MobileAdvisor.DAL.Data;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.DAL.Repositories.Implementations;

public class ConsultationRepository : GenericRepository<Consultation>, IConsultationRepository
{
    public ConsultationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Consultation?> GetConsultationWithDetailsAsync(int id)
    {
        return await _context.Consultations
            .Include(c => c.Customer)
            .Include(c => c.Expert)
            .Include(c => c.Attachments)
            .Include(c => c.Recommendation)
            .Include(c => c.Payment)
            .Include(c => c.Messages.OrderBy(m => m.SentAt))
                .ThenInclude(m => m.Sender)
            .Include(c => c.Review)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Consultation>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Consultations
            .Where(c => c.CustomerId == customerId)
            .Include(c => c.Expert)
            .Include(c => c.Recommendation)
            .Include(c => c.Payment)
            .Include(c => c.Review)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultation>> GetByExpertIdAsync(int expertId)
    {
        return await _context.Consultations
            .Where(c => c.ExpertId == expertId)
            .Include(c => c.Customer)
            .Include(c => c.Attachments)
            .Include(c => c.Payment)
            .Include(c => c.Review)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
