using Core.Entities;
using Core.Requests;

namespace Core;

public interface IUserManagementService
{
    public Task<Result<User>> RegisterUser(RegistrationRequest request);
}