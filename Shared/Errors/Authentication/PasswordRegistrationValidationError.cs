namespace Core.Errors.Authentication;

public class PasswordRegistrationValidationError(
    bool hasMinLength,
    bool hasUpperCase,
    bool hasLowerCase,
    bool hasDigit,
    bool hasSpecialChar) : IError
{
    private readonly string? _message = $"Password validation error \n" +
                                        $"{{" +
                                        $"hasMinLength: {hasMinLength}, \n" +
                                        $"hasUpperCase: {hasUpperCase}, \n" +
                                        $"hasLowerCase: {hasLowerCase}, \n" +
                                        $"hasDigit: {hasDigit}, \n" +
                                        $"hasSpecialChar: {hasSpecialChar}\n" +
                                        $"}}";
    public bool HasMinLength => hasMinLength;
    public bool HasUpperCase => hasUpperCase;
    public bool HasLowerCase => hasLowerCase;
    public bool HasDigit => hasDigit;
    public bool HasSpecialChar => hasSpecialChar;
    public string Message => _message!;
}