using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasOne(p => p.Consultation)
               .WithOne(c => c.Payment)
               .HasForeignKey<Payment>(p => p.ConsultationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Amount).HasPrecision(18, 2);
    }
}
