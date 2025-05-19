using Core.Entities;

namespace Core.Responses;

public class RegistrationResponse : IResponse
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTimeOffset RegisteredAt { get; set; }
}

public static class RegistrationResponseExtension
{
    public static RegistrationResponse ToRegistrationResponse(this User user)
    {
        return new RegistrationResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt
        };
    }
}