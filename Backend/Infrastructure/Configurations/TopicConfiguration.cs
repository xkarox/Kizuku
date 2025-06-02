using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasMany(t => t.SubTopics)
            .WithOne(t => t.ParentTopic)
            .HasForeignKey(t => t.ParentTopicId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Status)
            .WithMany()
            .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}