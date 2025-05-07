using Backend.Infrastructure;
using Core;
using Core.Entities;
using Core.Errors;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class UserRepository(
    IKizukuContext db
    ) : IUserRepository
{
    public async Task<Result<User>> Create(User entity)
    {
        if (entity == null)
            return Result<User>
                .Failure(new EntityNullError<User>());
        db.Users.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<User>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<User>>> GetAll()
    {
        try
        {
            var result = await db.Users.ToListAsync();
            return Result<IEnumerable<User>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<User>>.Failure(
                new DatabaseError(e.Message));
        }
    }

    public async Task<Result<User>> GetById(Guid id)
    {
        try
        {
            var query = db.Users.Where(u => u.UserId == id)
                .AsQueryable();
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.UserId), 
                        id.ToString()));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<User>> Get(User entity)
    {
        try
        {
            var query = db.Users.Where(u => u == entity)
                .AsQueryable();
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(new EntityNotFoundError<User>(entity));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Update(User entity)
    { 
        var entityToUpdate = await db.Users.Where(e => e.UserId == entity.UserId).FirstOrDefaultAsync(); 

        if (entityToUpdate == null)
        {
            return Result.Failure(new EntityNotFoundError<User>(entity)); 
        }
        
        entityToUpdate!.Username = entity.Username;
        entityToUpdate.Email = entity.Email;
        entityToUpdate.Password = entity.Password;
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to update user: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Delete(User entity)
    {
        var entityToDelete = await db.Users.Where(e => e.UserId == entity.UserId).FirstOrDefaultAsync(); 

        if (entityToDelete == null)
        {
            return Result.Failure(new EntityNotFoundError<User>(entity)); 
        }
        
        db.Users.Remove(entityToDelete);
        
        try
        {
            var opResult = await db.SaveChangesAsync();
            if (opResult == 0)
                return Result.Failure(new DatabaseError($"Failed to delete user: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<User>> GetByEmail(string email)
    {
        try
        {
            var query = db.Users.Where(u => u.Email == email);
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Email), email));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }
    
    public async Task<Result<User>> GetByUsername(string username)
    {
        try
        {
            var query = db.Users
                .Where(u => u.Username == username);
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Username), username));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new DatabaseError(e.Message));
        }
    }
}