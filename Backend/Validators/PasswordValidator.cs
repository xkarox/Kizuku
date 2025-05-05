using Core;
using Core.Errors.Authentication;
using Core.Validators;

namespace Backend.Validators;

public class PasswordValidator : IPasswordValidator
{
    public Result Validate(string password)
    {
        bool hasMinLength = password.Length >= 8;
        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

        if(!(hasMinLength && hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar))
            return Result.Failure(new PasswordRegistrationValidationError(
                hasMinLength,
                hasUpperCase,
                hasLowerCase,
                hasDigit,
                hasSpecialChar
                ));
        
        return Result.Success();
    }
}