using System.ComponentModel.DataAnnotations;

namespace Core.Requests;

public record LoginRequest
{
    [Required]
    public required string Email { get; init; }
    [Required]
    public required string Password { get; init; }
};