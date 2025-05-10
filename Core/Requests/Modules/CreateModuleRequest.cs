using Core.Entities;

namespace Core.Requests;

public record CreateModuleRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTime CreationDate { get; init; } = DateTime.UtcNow;
    public DateTime ModificationDate { get; init; } = DateTime.UtcNow;
}

public static class CreateModuleRequestExtensions
{
    public static Module ToModule(this CreateModuleRequest request, Guid userId)
    {
        return new Module()
        {
            Name = request.Name,
            Description = request.Description,
            UserId = userId,
            CreatedAt = request.CreationDate,
            UpdatedAt = request.ModificationDate,
        };
    }
}