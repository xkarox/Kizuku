using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Requests;

public record RegistrationRequest
{
    [MinLength(3), MaxLength(30)]
    public required string Username { get; init; }
    [MinLength(8), 
     RegularExpression("/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/")]
    public required string Password { get; set; }
    [EmailAddress]
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
}