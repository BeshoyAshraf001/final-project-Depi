using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<User> Users { get; }
    IExpertProfileRepository ExpertProfiles { get; }
    IGenericRepository<Specialty> Specialties { get; }
    IGenericRepository<ExpertSpecialty> ExpertSpecialties { get; }
    IConsultationRepository Consultations { get; }
    IGenericRepository<ConsultationAttachment> ConsultationAttachments { get; }
    IGenericRepository<ExpertRecommendation> ExpertRecommendations { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<Message> Messages { get; }
    IGenericRepository<Phone> Phones { get; }
    IGenericRepository<Review> Reviews { get; }
    IGenericRepository<Notification> Notifications { get; }

    Task<int> CompleteAsync();
}
