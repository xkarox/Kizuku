using Core.Entities;

namespace Core.Responses;

public record UserModulesResponse
{
    public Guid UserId { get; set; }
    public required ICollection<Module> Modules { get; set; }
}

public static class UserModulesResponseExtensions
{
    public static UserModulesResponse ToUserModulesResponse(
        this IEnumerable<Module> modules, Guid userId)
    {
        return new UserModulesResponse
        {
            UserId = userId,
            Modules = modules.ToList(),
        };
    }
}