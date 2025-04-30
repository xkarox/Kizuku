using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Core.Entities;

public class User
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
}