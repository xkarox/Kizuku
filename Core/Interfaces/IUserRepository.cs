using Core.Entities;

namespace Core;

public interface IUserRepository : IRepositoryBase<User> 
{
    public Task<Result<User>> GetUserByEmail(string email);
    public Task<Result<User>> GetUserByUsername(string username);
}