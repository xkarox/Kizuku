namespace Core.Responses;

public class ClaimResponse : IResponse
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}