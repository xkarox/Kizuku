using Core.Entities;

namespace Core.Requests;

public record UpdateModuleRequest()
{
    public required Guid ModuleId { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; }
};

public static class UpdateModuleRequestExtensions
{
    public static Module ToModule(this UpdateModuleRequest updateModuleRequest,
        Guid userId)
    {
        return new Module()
        {
            Id = updateModuleRequest.ModuleId,
            Name = updateModuleRequest.Name!,
            Description = updateModuleRequest.Description!,
            UpdatedAt = updateModuleRequest.UpdatedAt
        };
    }
}