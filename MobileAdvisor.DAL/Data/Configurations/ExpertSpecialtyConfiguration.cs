using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class ExpertSpecialtyConfiguration : IEntityTypeConfiguration<ExpertSpecialty>
{
    public void Configure(EntityTypeBuilder<ExpertSpecialty> builder)
    {
        builder.HasKey(es => new { es.ExpertProfileId, es.SpecialtyId });

        builder.HasOne(es => es.ExpertProfile)
               .WithMany(ep => ep.ExpertSpecialties)
               .HasForeignKey(es => es.ExpertProfileId);

        builder.HasOne(es => es.Specialty)
               .WithMany(s => s.ExpertSpecialties)
               .HasForeignKey(es => es.SpecialtyId);
    }
}
