namespace Core.Errors.Authentication;

public class InvalidPasswordError : IError
{
    private readonly string _message = "Invalid password provided";

    public string Message => _message;
}