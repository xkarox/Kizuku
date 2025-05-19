using Backend.Infrastructure;
using Core;
using Core.Entities;
using Core.Errors.Authentication;
using Core.Errors.Database;
using Core.Errors.Entities;
using Core.Requests;
using Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ModuleService(
    IKizukuContext db,
    IModuleRepository moduleRepository,
    ILogger<ModuleService> logger
    )
    : IModuleService
{
    public async Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId)
    {
        var moduleResult =  await moduleRepository.GetAllByUserId(userId);
        if (moduleResult.IsError)
        {
            logger.LogError($"Failed to get User modules for: {userId}");
            return Result<IEnumerable<Module>>.Failure(moduleResult.Error);
        }
        return moduleResult;
    }

    public async Task<Result<Module>> CreateUserModule(CreateModuleRequest createModuleRequest, Guid userId)
    {
        var module = createModuleRequest.ToModule(userId);
        var result = await moduleRepository.Create(module);
        if (result.IsError)
        {
            logger.LogError($"Failed to create module for: {userId}");
            return Result<Module>.Failure(result.Error);
        }
        return Result<Module>.Success(module);
    }

    public async Task<Result<Module>> UpdateUserModule(UpdateModuleRequest updateModuleRequest, Guid userId)
    {
        var module = updateModuleRequest.ToModule(userId);
        var result = await moduleRepository.Update(module);
        if (result.IsError)
        {
            logger.LogError($"Failed to update module for: {userId}");
            return Result<Module>.Failure(result.Error);
        }
        return Result<Module>.Success(module);
    }
    
    public async Task<Result> DeleteUserModule(Guid moduleId, Guid userId)
    {
        var result = await moduleRepository.Delete(new Module() { Id = moduleId, Name = "Name" });
        if (result.IsError)
        {
            logger.LogError($"Failed to delete module for: {userId}");
            return Result.Failure(result.Error);
        }
        return Result.Success();
    }
    
}