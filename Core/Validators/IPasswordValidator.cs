namespace Core.Validators;

public interface IPasswordValidator
{
    /// <summary>
/// Validates the specified password and returns the result of the validation.
/// </summary>
/// <param name="password">The password to validate.</param>
/// <returns>A <see cref="Result"/> indicating whether the password meets validation criteria.</returns>
public Result Validate(string password);
}