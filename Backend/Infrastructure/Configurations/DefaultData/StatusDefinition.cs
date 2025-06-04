using Core.Entities;

namespace Backend.Infrastructure.Configurations.DefaultData;

public class StatusDefinition
{
    public static Status NotStarted = new Status
    {
        Id = new Guid("029ceaa3-97c0-42ed-b61c-55dc80071d1e"),
        Name =
            "Not Started" /*, Description = "Work has not yet begun."*/
    };

    public static Status InProgress = new Status
    {
        Id = new Guid("e9e2dd4e-89d1-491d-82c7-073167dd0006"),
        Name =
            "In Progress" /*, Description = "Currently being worked on."*/
    };

    public static Status NeedsReview = new Status
    {
        Id = new Guid("7c779bf9-b7ec-4b47-bbdd-e4cdadb2d535"),
        Name =
            "Needs Review" /*, Description = "Completed but requires checking."*/
    };

    public static Status Completed = new Status
    {
        Id = new Guid("208bb40d-21b3-443e-a1d5-a37685c27912"),
        Name = "Completed" /*, Description = "All work finished."*/
    };

    public static Status Repeat = new Status
    {
        Id = new Guid("d684c78b-0b73-4e73-90af-8a900e1c9aad"),
        Name = "Repeat" /*, Description = "Needs to be studied again."*/
    };

    public static IEnumerable<Status> States = new List<Status>
    {
        NotStarted, InProgress, NeedsReview, Completed, Repeat
    };
}