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
    IModuleRepository moduleRepository
    )
    : IModuleService
{
    public async Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId)
    {
        try
        {
            var query = db.Modules.Where(u => u.UserId == userId)
                .AsQueryable();
            var result = await query.ToListAsync();
            return Result<IEnumerable<Module>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<Module>>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Module>> CreateUserModule(CreateModuleRequest createModuleRequest, Guid userId)
    {
        var module = createModuleRequest.ToModule(userId);
        db.Modules.Add(module);
        try
        {
            await db.SaveChangesAsync();
            return Result<Module>.Success(module);
        }
        catch (DbUpdateException e)
        {
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Module>> UpdateUserModule(UpdateModuleRequest updateModuleRequest, Guid userId)
    {
        var module = await db.Modules.FirstOrDefaultAsync(u => u.Id == updateModuleRequest.ModuleId);
        if (module == null)
            return Result<Module>.Failure(new EntityNotFoundError<Module>(updateModuleRequest.ModuleId));
        var moduleUserId = module.UserId;
        if (moduleUserId != userId)
            return Result<Module>.Failure(new CrudOperationOwnershipError());
        
        module.Name = updateModuleRequest.Name ?? module.Name;
        module.Description = updateModuleRequest.Description ?? module.Description;
        module.UpdatedAt = updateModuleRequest.UpdatedAt;

        try
        {
            await db.SaveChangesAsync();
            return Result<Module>.Success(module);
        }
        catch (DbUpdateException e)
        {
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }
    
    public async Task<Result<Module>> DeleteUserModule(Guid moduleId, Guid userId)
    {
        var module = db.Modules.FirstOrDefault(u => u.Id == moduleId);
        if (module == null)
            return Result<Module>.Failure(new EntityNotFoundError<Module>(moduleId));
        var moduleUserId = module.UserId;
        if (moduleUserId != userId)
            return Result<Module>.Failure(new CrudOperationOwnershipError());
        db.Modules.Remove(module);
        try
        {
            await db.SaveChangesAsync();
            return Result<Module>.Success(module);
        }
        catch (DbUpdateException e)
        {
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }
    
}