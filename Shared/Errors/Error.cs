namespace Core.Errors;

public class Error(string? message = null) : IError
{
    private readonly string _message = message ?? "Default Error";

    public string Message
    {
        get => _message;
        init => _message = value ?? "Default Error";
    }
}