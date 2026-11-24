using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<ExpertProfile> ExpertProfiles => Set<ExpertProfile>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<ExpertSpecialty> ExpertSpecialties => Set<ExpertSpecialty>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<ConsultationAttachment> ConsultationAttachments => Set<ConsultationAttachment>();
    public DbSet<ExpertRecommendation> ExpertRecommendations => Set<ExpertRecommendation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Phone> Phones => Set<Phone>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
