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

public class StudyManagementService(
    IKizukuContext db,
    ILogger<StudyManagementService> logger
    )
    : IStudyManagementService
{
    public async Task<Result<IEnumerable<Module>>> GetUserModules(Guid userId)
    {
        var moduleResult =  await GetAllByUserId(userId);
        if (moduleResult.IsError)
        {
            logger.LogError($"Failed to get User modules for: {userId}");
            return Result<IEnumerable<Module>>.Failure(moduleResult.Error);
        }
        return moduleResult;
    }

    public async Task<Result<Module>> GetModuleWithTopics(Guid moduleId)
    {
        try
        {
            var query = 
                    db.Modules.Where(module => module.Id == moduleId)
                        .Include(module => module.Topics)
                .AsQueryable();
            var module = await query.FirstOrDefaultAsync();
            if (module is null)
                return Result<Module>.Failure(new DatabaseError("Module not found"));
            return Result<Module>.Success(module);
        }
        catch (ArgumentNullException e)
        {
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }
    
    public async Task<Result<Module>> AddTopicToModule(AddTopicToModuleRequest request)
    {
        try
        {
            var query = 
                db.Modules.Where(module => module.Id == request.ModuleId)
                    .Include(module => module.Topics)
                    .AsQueryable();
            var module = await query.FirstOrDefaultAsync();
            if (module is null)
                return Result<Module>.Failure(new DatabaseError("Module not found"));
            
            module.Topics.Add(new Topic
            {
                Name = request.TopicName,
                Description = request.TopicDescription,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            
            await db.SaveChangesAsync();
            return Result<Module>.Success(module);
        }
        catch (ArgumentNullException e)
        {
            logger.LogError(e.Message);
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e.Message);
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Module>> CreateUserModule(CreateModuleRequest createModuleRequest, Guid userId)
    {
        var module = createModuleRequest.ToModule(userId);
        var result = await Create(module);
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
        var result = await Update(module);
        if (result.IsError)
        {
            logger.LogError($"Failed to update module for: {userId}");
            return Result<Module>.Failure(result.Error);
        }
        return Result<Module>.Success(module);
    }
    
    public async Task<Result> DeleteUserModule(Guid moduleId, Guid userId)
    {
        var result = await Delete(new Module() { Id = moduleId, Name = "Name" });
        if (result.IsError)
        {
            logger.LogError($"Failed to delete module for: {userId}");
            return Result.Failure(result.Error);
        }
        return Result.Success();
    }
    
    private async Task<Result<Module>> Create(Module entity)
    {
        if (entity == null)
            return Result<Module>
                .Failure(new EntityNullError<Module>());
        db.Modules.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<Module>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e.Message);
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }
    private async Task<Result<IEnumerable<Module>>> GetAllByUserId(Guid userId)
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
    
    private async Task<Result> Update(Module entity)
    { 
        var entityToUpdate = await db.Modules.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToUpdate == null)
        {
            return Result.Failure(new EntityNotFoundError<Module>(entity)); 
        }
        
        entityToUpdate!.Name = entity.Name ?? entityToUpdate.Name;
        entityToUpdate.Description = entity.Description ?? entityToUpdate.Description;
        entityToUpdate.UpdatedAt = entity.UpdatedAt;
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to update Module: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }
    
    private async Task<Result> Delete(Module entity)
    {
        var entityToDelete = await db.Modules.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToDelete == null)
        {
            return Result.Failure(new EntityNotFoundError<Module>(entity)); 
        }
        
        db.Modules.Remove(entityToDelete);
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to delete Module: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }
}