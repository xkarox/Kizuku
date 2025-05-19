using System.Security.Claims;
using Core.Entities;

namespace Core;

public interface IAuthenticationService
{
    public Task<Result<User>> ValidateCredentials(string email, string password);
    public ClaimsPrincipal GetClaimsPrincipal(User user);
    public string HashPassword(string password);
}