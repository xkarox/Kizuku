using System.Security.Claims;
using Backend.Infrastructure;
using Core;
using Core.Entities;
using Core.Errors.Authentication;
using Core.Errors.Database;
using Core.Errors.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
using IAuthenticationService = Core.IAuthenticationService;

namespace Backend.Services;

public class AuthenticationService(
    IKizukuContext db
    ): IAuthenticationService
{
    public async Task<Result<User>> ValidateCredentials(string email, 
        string password)
    {
        var getUserByEmailResult = await GetByEmail(email);
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
    
    private async Task<Result<User>> GetByEmail(string email)
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
}