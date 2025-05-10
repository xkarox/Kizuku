

using Core.Entities;

namespace Core.Requests;

public record DeleteModuleRequest()
{
    public Guid ModuleId { get; init; }
};

public static class DeleteModuleRequestExtensions
{
    public static DeleteModuleRequest ToDeleteModuleRequest(
        this Module module)
    {
        return new DeleteModuleRequest()
        {
            ModuleId = module.Id
        };
    }
}