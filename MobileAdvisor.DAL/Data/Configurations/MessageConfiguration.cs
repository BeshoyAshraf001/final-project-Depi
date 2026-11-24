using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileAdvisor.DAL.Entities;

namespace MobileAdvisor.DAL.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(m => m.Consultation)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.ConsultationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Sender)
               .WithMany(u => u.SentMessages)
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
