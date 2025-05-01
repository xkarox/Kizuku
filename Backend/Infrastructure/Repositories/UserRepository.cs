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
        var query = db.Users.AsQueryable();
        try
        {
            var result = await query.ToListAsync();
            return Result<IEnumerable<User>>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<IEnumerable<User>>.Failure(
                new EntityNullError<KizukuContext>());
        }
    }

    public async Task<Result<User>> GetById(Guid id)
    {
        var query = db.Users.Where(u => u.UserId == id)
            .AsQueryable();
        try
        {
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.UserId), 
                        id.ToString()));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new EntityNullError<KizukuContext>());
        }
    }

    public async Task<Result<User>> Get(User entity)
    {
        var query = db.Users.Where(u => u.Equals(entity))
            .AsQueryable();
        try
        {
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(new EntityNotFoundError<User>(entity));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new EntityNullError<KizukuContext>());
        }
    }

    public async Task<Result> Update(User entity)
    { 
        db.Users.Update(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result> Delete(User entity)
    {
        db.Users.Remove(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    public async Task<Result<User>> GetUserByEmail(string email)
    {
        var query = db.Users.AsQueryable()
            .Where(u => u.Email == email);
        try
        {
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Email), email));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new EntityNullError<KizukuContext>());
        }
    }
    
    public async Task<Result<User>> GetUserByUsername(string username)
    {
        var query = db.Users.AsQueryable()
            .Where(u => u.Username == username);
        try
        {
            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return Result<User>.Failure(
                    new EntityNotFoundError<User>(nameof(User.Username), username));
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(new EntityNullError<KizukuContext>());
        }
    }
}