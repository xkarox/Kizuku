using Core.Entities;

namespace Core;

public interface IUserRepository : IRepositoryBase<User> 
{
    /// <summary>
/// Asynchronously retrieves a user by their email address.
/// </summary>
/// <param name="email">The email address of the user to retrieve.</param>
/// <returns>A task that resolves to a result containing the user if found, or an error if not.</returns>
public Task<Result<User>> GetByEmail(string email);
    /// <summary>
/// Asynchronously retrieves a user by their username.
/// </summary>
/// <param name="username">The username to search for.</param>
/// <returns>A task that resolves to a result containing the user if found, or an error if not.</returns>
public Task<Result<User>> GetByUsername(string username);
public Task<Result<IEnumerable<User>>> GetAll();
public Task<Result<User>> GetById(Guid id);
}