using Core.Entities;
using Core.Requests;

namespace Core;

public interface IUserService
{
    public Task<Result<User>> RegisterUser(RegistrationRequest request);
}