using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ModuleRepository(
    IKizukuContext db
    ) : IModuleRepository
{
     public async Task<Result<Module>> Create(Module entity)
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
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<Module>>> GetAll()
    {
        try
        {
            var result = await db.Modules.ToListAsync();
            return Result<IEnumerable<Module>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<Module>>.Failure(
                new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Module>> Get(Module entity)
    {
        try
        {
            var query = db.Modules.Where(m => m.Id == entity.Id)
                .AsQueryable();
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<Module>.Failure(new EntityNotFoundError<Module>(entity));
            return Result<Module>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<Module>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Update(Module entity)
    { 
        var entityToUpdate = await db.Modules.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToUpdate == null)
        {
            return Result.Failure(new EntityNotFoundError<Module>(entity)); 
        }
        
        entityToUpdate!.Name = entity.Name;
        entityToUpdate.Description = entity.Description;
        entityToUpdate.CreatedAt = entity.CreatedAt;
        entityToUpdate.UpdatedAt = entity.UpdatedAt;
        entityToUpdate.Topics = entity.Topics;
        
        entityToUpdate.UserId = entity.UserId;
        entityToUpdate.User = entity.User;
        
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

    public async Task<Result> Delete(Module entity)
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