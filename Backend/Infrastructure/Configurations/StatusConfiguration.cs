using Backend.Infrastructure.Configurations.DefaultData;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();
        
        builder.HasMany(s => s.Topics)
            .WithOne(s => s.Status)
            .HasForeignKey(s => s.StatusId);
        
        builder.HasData(
            StatusDefinition.States
        );
    }
}