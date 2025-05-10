using Core;
using Core.Entities;
using Core.Responses;

namespace Backend.Services;

public class ModuleService : IModuleService
{
    public Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId)
    {
        throw new NotImplementedException();
    }
}