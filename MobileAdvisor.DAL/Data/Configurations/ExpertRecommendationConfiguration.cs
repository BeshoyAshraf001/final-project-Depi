using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class ExpertRecommendationConfiguration : IEntityTypeConfiguration<ExpertRecommendation>
{
    public void Configure(EntityTypeBuilder<ExpertRecommendation> builder)
    {
        builder.HasOne(r => r.Consultation)
               .WithOne(c => c.Recommendation)
               .HasForeignKey<ExpertRecommendation>(r => r.ConsultationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
