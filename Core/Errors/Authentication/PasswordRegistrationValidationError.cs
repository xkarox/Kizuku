namespace Core.Errors.Authentication;

public class PasswordRegistrationValidationError: IError
{
    private readonly string? _message;
    public string Message { get; }

    public PasswordRegistrationValidationError(
        bool hasMinLength,
        bool hasUpperCase,
        bool hasLowerCase,
        bool hasDigit,
        bool hasSpecialChar)
    {
        _message = $"Password validation error \n" +
                   $"{{" +
                   $"hasMinLength: {hasMinLength}, \n" +
                   $"hasUpperCase: {hasUpperCase}, \n" +
                   $"hasLowerCase: {hasLowerCase}, \n" +
                   $"hasDigit: {hasDigit}, \n" +
                   $"hasSpecialChar: {hasSpecialChar}\n" +
                   $"}}";
    }
}