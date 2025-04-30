using Backend.Infrastructure;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(
    KizukuContext db
    ) : IRepository<User>
{
    public async Task<Result<User>> Create(User entity)
    {
        if (entity == null)
            return Result<User>
                .Failure(new ArgumentNullException(nameof(entity)).Message);
        db.Users.Add(entity);
        try
        {
            await db.SaveChangesAsync();
            return Result<User>.Success(entity);
        }
        catch (DbUpdateException e)
        {
            return Result<User>.Failure(e.Message);
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
            return Result<IEnumerable<User>>.Failure(e.Message);
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
                return Result<User>.Failure("User not found");
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(e.Message);
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
                return Result<User>.Failure("User not found");
            return Result<User>.Success(result);
        }
        catch (ArgumentNullException e)
        {
            return Result<User>.Failure(e.Message);
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
            return Result.Failure(e.Message);
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
            return Result.Failure(e.Message);
        }
    }
}