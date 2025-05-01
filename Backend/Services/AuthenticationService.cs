using System.Security.Claims;
using Backend.Repositories;
using Core;
using Core.Entities;
using Core.Errors.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BC = BCrypt.Net.BCrypt;
using IAuthenticationService = Core.IAuthenticationService;

namespace Backend.Services;

public class AuthenticationService(
    IUserRepository userRepository
    ): IAuthenticationService
{
    public async Task<Result<User>> ValidateCredentials(string email, 
        string password)
    {
        var getUserByEmailResult = await userRepository.GetUserByEmail(email);
        if (getUserByEmailResult.IsError)
            return Result<User>.Failure(getUserByEmailResult.Error);

        var user = getUserByEmailResult.Data;
        var passwordHash = BC.HashPassword(password, 12);
        
        return !VerifyPassword(passwordHash, user!.Password) 
            ? Result<User>.Failure(new InvalidPasswordError()) 
            : Result<User>.Success(user);
    }

    public ClaimsPrincipal GetClaimsPrincipal(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user!.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User"),
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
        return BC.Verify(passwordHash, password);
    }
}