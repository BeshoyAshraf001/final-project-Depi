using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class ExpertProfileConfiguration : IEntityTypeConfiguration<ExpertProfile>
{
    public void Configure(EntityTypeBuilder<ExpertProfile> builder)
    {
        builder.HasOne(e => e.User)
               .WithOne(u => u.ExpertProfile)
               .HasForeignKey<ExpertProfile>(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.DefaultFee).HasPrecision(18, 2);
    }
}
