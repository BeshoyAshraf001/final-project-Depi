using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.HasOne(c => c.Customer)
               .WithMany(u => u.CustomerConsultations)
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Expert)
               .WithMany(u => u.ExpertConsultations)
               .HasForeignKey(c => c.ExpertId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.Budget).HasPrecision(18, 2);
        builder.Property(c => c.UsedPhoneAskingPrice).HasPrecision(18, 2);
        builder.Property(c => c.AgreedPrice).HasPrecision(18, 2);
    }
}
