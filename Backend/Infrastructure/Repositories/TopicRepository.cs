using Core;
using Core.Entities;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TopicRepository
    (IKizukuContext db) : ITopicRepository
{
    public async Task<Result<Topic>> Create(Topic entity)
    {
        if (entity == null)
            return Result<Topic>
                .Failure(new EntityNullError<Topic>());
        db.Topics.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<Topic>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            return Result<Topic>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<Topic>>> GetAll()
    {
        try
        {
            var result = await db.Topics.ToListAsync();
            return Result<IEnumerable<Topic>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<Topic>>.Failure(
                new DatabaseError(e.Message));
        }
    }

    public async Task<Result<Topic>> Get(Topic entity)
    {
        try
        {
            var query = db.Topics.Where(u => u == entity)
                .AsQueryable();
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<Topic>.Failure(new EntityNotFoundError<Topic>(entity));
            return Result<Topic>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<Topic>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Update(Topic entity)
    { 
        var entityToUpdate = await db.Topics.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToUpdate == null)
        {
            return Result.Failure(new EntityNotFoundError<Topic>(entity)); 
        }
        
        entityToUpdate!.Name = entity.Name;
        entityToUpdate.Topics = entity.Topics;
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to update Topic: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Delete(Topic entity)
    {
        var entityToDelete = await db.Topics.Where(e => e.Id == entity.Id).FirstOrDefaultAsync(); 

        if (entityToDelete == null)
        {
            return Result.Failure(new EntityNotFoundError<Topic>(entity)); 
        }
        
        db.Topics.Remove(entityToDelete);
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to delete Topic: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }
}