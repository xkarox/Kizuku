using Core.Entities;

namespace Core.Responses;

public record GetModulesResponse : IResponse
{
    public Guid UserId { get; init; }
    public required ICollection<Module> Modules { get; init; }
}

public static class UserModulesResponseExtensions
{
    public static GetModulesResponse ToUserModulesResponse(
        this IEnumerable<Module> modules, Guid userId)
    {
        return new GetModulesResponse
        {
            UserId = userId,
            Modules = modules.ToList(),
        };
    }
}