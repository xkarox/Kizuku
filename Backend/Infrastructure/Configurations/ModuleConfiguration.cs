using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.Property(m => m.Description).HasMaxLength(1000);
        builder.HasMany(m => m.Topics)
            .WithOne(t => t.Module)
            .HasForeignKey(t => t.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}