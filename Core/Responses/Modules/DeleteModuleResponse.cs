using Core.Entities;
using Core.Requests;

namespace Core.Responses;

public record DeleteModuleResponse()
{
    public Guid ModuleId { get; init; }
};

public static class DeleteModuleResponseExtensions
{
    public static DeleteModuleResponse ToDeleteModuleResponse(
        this Module module)
    {
        return new DeleteModuleResponse()
        {
            ModuleId = module.Id,
        };
    }
}