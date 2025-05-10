using Core.Entities;
using Core.Responses;

namespace Core;

public interface IModuleService
{
    public Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId);
}