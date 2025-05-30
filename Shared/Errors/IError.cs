namespace Core.Errors;

public interface IError
{
    public string Code => GetType().Name;
    public string Message { get; }
}