namespace Core.Errors.Authentication;

public class PasswordValidationError : IError
{
    private readonly string _message = "Invalid password provided";

    public string Message => _message;
}