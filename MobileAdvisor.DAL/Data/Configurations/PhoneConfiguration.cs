using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.Property(p => p.GuidePrice).HasPrecision(18, 2);
        builder.Property(p => p.Brand).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Model).HasMaxLength(150).IsRequired();
    }
}
