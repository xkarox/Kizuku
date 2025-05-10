using System.Security.Claims;
using Core;
using Core.Entities;
using Core.Errors.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BC = BCrypt.Net.BCrypt;
using IAuthenticationService = Core.IAuthenticationService;

namespace Backend.Services;

public class AuthenticationService(
    IUserRepository userRepository
    ): IAuthenticationService
{
    /// <summary>
    /// Asynchronously validates a user's credentials by checking the provided email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The plaintext password to validate.</param>
    /// <returns>A result containing the user if authentication succeeds; otherwise, a failure result with the relevant error.</returns>
    public async Task<Result<User>> ValidateCredentials(string email, 
        string password)
    {
        var getUserByEmailResult = await userRepository.GetByEmail(email);
        if (getUserByEmailResult.IsError)
            return Result<User>.Failure(getUserByEmailResult.Error);

        var user = getUserByEmailResult.Value;
        
        return !VerifyPassword(password, user!.Password) 
            ? Result<User>.Failure(new PasswordValidationError()) 
            : Result<User>.Success(user);
    }

    public ClaimsPrincipal GetClaimsPrincipal(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user!.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        };
        
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(claimsIdentity);
    }

    public string HashPassword(string password)
    {
        return BC.HashPassword(password, 12);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}