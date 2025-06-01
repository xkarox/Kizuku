using Core.Entities;

namespace Core.Requests;

public record AddTopicToModuleRequest
{
    public required Guid ModuleId { get; init; }
    public required Topic Topic { get; init; }
};