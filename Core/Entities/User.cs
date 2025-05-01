using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Index(nameof(Email), IsUnique = true), 
 Index(nameof(Username), IsUnique = true)]
public class User : IEntity
{
    [Key]
    public Guid UserId { get; set; }
    [MinLength(3), MaxLength(30)]
    public required string Username { get; set; }
    [MinLength(8)]
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public DateTime RegisteredAt { get; set; }

    public override string ToString()
    {
        return 
            $"{{ UserId: {UserId}, User: {Username}, Email: {Email}, RegisteredAt: {RegisteredAt} }}";
    }
}