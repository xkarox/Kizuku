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
    /// <summary>
    /// Asynchronously adds a new User entity to the database.
    /// </summary>
    /// <param name="entity">The User entity to add.</param>
    /// <returns>A Result containing the created User on success, or an error if the operation fails.</returns>
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

    /// <summary>
    /// Asynchronously retrieves all User entities from the database.
    /// </summary>
    /// <returns>A Result containing the list of all User entities, or a failure with a DatabaseError if an error occurs.</returns>
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

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A result containing the user if found, or a failure with an error if not found or if a database error occurs.</returns>
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

    /// <summary>
    /// Retrieves a user from the database that matches the provided entity.
    /// </summary>
    /// <param name="entity">The user entity to match against existing records.</param>
    /// <returns>A result containing the found user if successful; otherwise, a failure with an appropriate error.</returns>
    public async Task<Result<User>> Get(User entity)
    {
        try
        {
            var query = db.Users.Where(u => u.Equals(entity))
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

    /// <summary>
    /// Updates the username, email, and password of an existing User entity in the database.
    /// </summary>
    /// <param name="entity">The User entity containing updated values. The UserId is used to locate the existing record.</param>
    /// <returns>A Result indicating success, or failure with an EntityNotFoundError if the user does not exist, or a DatabaseError if the update fails.</returns>
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
            if (opResult != 1)
                return Result.Failure(new DatabaseError($"Failed to update user: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    /// <summary>
    /// Deletes a user entity from the database if it exists.
    /// </summary>
    /// <param name="entity">The user entity to delete, identified by its UserId.</param>
    /// <returns>A result indicating success, or failure with an error if the user is not found or the deletion fails.</returns>
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
            if (opResult != 1)
                return Result.Failure(new DatabaseError($"Failed to delete user: {entity}"));
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(new DatabaseError(e.Message));
        }
    }

    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>A result containing the found user, or a failure with an error if not found or if a database error occurs.</returns>
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
    
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>A result containing the user if found, or a failure with an appropriate error if not found or if a database error occurs.</returns>
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