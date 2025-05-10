namespace Core.Responses;

public class UserInfoResponse : IResponse
{
    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<ClaimResponse> Claims { get; set; } = new();
}