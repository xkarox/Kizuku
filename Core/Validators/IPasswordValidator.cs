namespace Core.Validators;

public interface IPasswordValidator
{
    public Result Validate(string password);
}