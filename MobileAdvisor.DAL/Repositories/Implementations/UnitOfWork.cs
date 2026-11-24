using MobileAdvisor.DAL.Data;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Repositories.Interfaces;

namespace MobileAdvisor.DAL.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IGenericRepository<User> Users { get; }
    public IExpertProfileRepository ExpertProfiles { get; }
    public IGenericRepository<Specialty> Specialties { get; }
    public IGenericRepository<ExpertSpecialty> ExpertSpecialties { get; }
    public IConsultationRepository Consultations { get; }
    public IGenericRepository<ConsultationAttachment> ConsultationAttachments { get; }
    public IGenericRepository<ExpertRecommendation> ExpertRecommendations { get; }
    public IGenericRepository<Payment> Payments { get; }
    public IGenericRepository<Message> Messages { get; }
    public IGenericRepository<Phone> Phones { get; }
    public IGenericRepository<Review> Reviews { get; }
    public IGenericRepository<Notification> Notifications { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new GenericRepository<User>(_context);
        ExpertProfiles = new ExpertProfileRepository(_context);
        Specialties = new GenericRepository<Specialty>(_context);
        ExpertSpecialties = new GenericRepository<ExpertSpecialty>(_context);
        Consultations = new ConsultationRepository(_context);
        ConsultationAttachments = new GenericRepository<ConsultationAttachment>(_context);
        ExpertRecommendations = new GenericRepository<ExpertRecommendation>(_context);
        Payments = new GenericRepository<Payment>(_context);
        Messages = new GenericRepository<Message>(_context);
        Phones = new GenericRepository<Phone>(_context);
        Reviews = new GenericRepository<Review>(_context);
        Notifications = new GenericRepository<Notification>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
