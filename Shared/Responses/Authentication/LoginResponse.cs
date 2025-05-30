using Core.Entities;

namespace Core.Responses;

public record LoginResponse : IResponse
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTimeOffset LoginTime { get; set; }
}

public static class LoginResponseExtension
{
    public static LoginResponse ToLoginResponse(this User user)
    {
        return new LoginResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            LoginTime = DateTimeOffset.UtcNow
        };
    }
}