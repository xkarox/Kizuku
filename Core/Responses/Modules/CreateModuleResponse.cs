using Core.Entities;

namespace Core.Responses;

public record CreateModuleResponse : IResponse
{
    public required Module Module { get; init; }
}

public static class CreateModuleResponseExtensions
{
    public static CreateModuleResponse ToCreateModuleResponse(
        this Module module)
    {
        return new CreateModuleResponse
        {
            Module = module
        };
    }
}