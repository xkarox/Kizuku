using Core.Entities;
using Core.Requests;
using Core.Responses;

namespace Core;

public interface IModuleService
{
    public Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId);
    public Task<Result<Module>> CreateUserModule(CreateModuleRequest createModuleRequest, Guid userId);
    public Task<Result<Module>> UpdateUserModule(UpdateModuleRequest updateModuleRequest, Guid userId);
    public Task<Result<Module>> DeleteUserModule(DeleteModuleRequest deleteModuleRequest, Guid userId);
}