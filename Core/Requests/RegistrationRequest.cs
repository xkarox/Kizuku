using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Requests;

public record RegistrationRequest
{
    [MinLength(3), MaxLength(30), Required]
    public required string Username { get; init; }
    [MinLength(8), Required]
    public required string Password { get; set; }
    [EmailAddress, Required]
    public required string Email { get; set; }
};

public static class RegistrationRequestExtensions
{
    public static User ToUser(this RegistrationRequest request,
        string passwordHash)
    {
        return new User
        {
            Username = request.Username,
            Password = passwordHash,
            Email = request.Email,
            RegisteredAt = DateTime.Now
        };
    }
    
    public static User ToUser(this RegistrationRequest request,
        Func<string, string> hashPasswordFunc)
    {
        return new User
        {
            Username = request.Username,
            Password = hashPasswordFunc(request.Password),
            Email = request.Email,
            RegisteredAt = DateTime.Now
        };
    }
}