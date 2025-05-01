namespace Core.Errors.Database;

public class DatabaseError(string message) : IError
{
    private readonly string _message = message;

    public string Message
    {
        get => _message; 
        init => _message = value;
    }
}