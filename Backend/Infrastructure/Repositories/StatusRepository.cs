using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class StatusRepository(
    IKizukuContext db
    ) : IStatusRepository
{
    public async Task<Result<Status>> Create(Status entity)
    {
        if (entity == null)
            return Result<Status>
                .Failure(new EntityNullError<Status>());
        db.Statuses.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<Status>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            return Result<Status>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<Status>>> GetAll()
    {
        try
        {
            var result = await db.Statuses.ToListAsync();
            return Result<IEnumerable<Status>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<Status>>.Failure(
                new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Status>> Get(Status entity)
    {
        try
        {
            var query = db.Statuses.Where(s => s.Id == entity.Id)
                .AsQueryable();
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<Status>.Failure(new EntityNotFoundError<Status>(entity));
            return Result<Status>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<Status>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Update(Status entity)
    { 
        var entityToUpdate = await db.Statuses.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToUpdate == null)
        {
            return Result.Failure(new EntityNotFoundError<Status>(entity)); 
        }
        
        entityToUpdate!.Name = entity.Name;
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to update Status: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Delete(Status entity)
    {
        var entityToDelete = await db.Statuses.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToDelete == null)
        {
            return Result.Failure(new EntityNotFoundError<Status>(entity)); 
        }
        
        db.Statuses.Remove(entityToDelete);
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to delete Status: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }
}