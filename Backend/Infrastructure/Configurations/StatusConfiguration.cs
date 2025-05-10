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
        
        var notStartedId = new Guid("029ceaa3-97c0-42ed-b61c-55dc80071d1e"); 
        var inProgressId = new Guid("e9e2dd4e-89d1-491d-82c7-073167dd0006");
        var needsReviewId = new Guid("7c779bf9-b7ec-4b47-bbdd-e4cdadb2d535");
        var completedId = new Guid("208bb40d-21b3-443e-a1d5-a37685c27912");
        var repeatId = new Guid("d684c78b-0b73-4e73-90af-8a900e1c9aad");
        builder.HasData(
            new Status
            {
                Id = notStartedId,
                Name =
                    "Not Started" /*, Description = "Work has not yet begun."*/
            },
            new Status
            {
                Id = inProgressId,
                Name =
                    "In Progress" /*, Description = "Currently being worked on."*/
            },
            new Status
            {
                Id = needsReviewId,
                Name =
                    "Needs Review" /*, Description = "Completed but requires checking."*/
            },
            new Status
            {
                Id = completedId,
                Name = "Completed" /*, Description = "All work finished."*/
            },
            new Status
            {
                Id = repeatId,
                Name = "Repeat" /*, Description = "Needs to be studied again."*/
            }
        );
    }
}