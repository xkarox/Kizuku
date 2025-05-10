using Core.Entities;

namespace Core.Requests;

public record UpdateModuleRequest()
{
    public required Guid ModuleId { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; }
};