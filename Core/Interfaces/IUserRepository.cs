using Core.Entities;

namespace Core;

public interface IUserRepository : IRepositoryBase<User> 
{
    public Task<Result<User>> GetByEmail(string email);
    public Task<Result<User>> GetByUsername(string username);
}