using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasOne(r => r.Consultation)
               .WithOne(c => c.Review)
               .HasForeignKey<Review>(r => r.ConsultationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Customer)
               .WithMany(u => u.GivenReviews)
               .HasForeignKey(r => r.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Expert)
               .WithMany(u => u.ReceivedReviews)
               .HasForeignKey(r => r.ExpertId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
