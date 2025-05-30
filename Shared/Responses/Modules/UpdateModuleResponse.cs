using Core.Entities;

namespace Core.Responses;

public class UpdateModuleResponse : IResponse
{
    public required Guid ModuleId { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class UpdateModuleResponseExtensions
{
    public static UpdateModuleResponse ToUpdateModuleResponse(
        this Module module)
    {
        return new UpdateModuleResponse
        {
            ModuleId = module.Id,
            Name = module.Name,
            Description = module.Description,
            UpdatedAt = module.UpdatedAt
        };
    }

    public static Module ToModule(
        this UpdateModuleResponse updateModuleResponse)
    {
        return new Module
        {
            Id = updateModuleResponse.ModuleId,
            Name = updateModuleResponse.Name,
            Description = updateModuleResponse.Description,
            UpdatedAt = updateModuleResponse.UpdatedAt
        };
    }
}