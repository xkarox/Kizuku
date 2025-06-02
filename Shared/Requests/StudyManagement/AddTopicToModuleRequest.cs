using Core.Entities;

namespace Core.Requests;

public record AddTopicToModuleRequest
{
    public required Guid ModuleId { get; init; }
    public required string TopicName { get; init; }
    public required string TopicDescription { get; init; }
};