namespace Core.Errors.Entities;

public class EntityNullError : IError
{
    public readonly string _message = "Provided entity is null";
    public string Message
    {
        get;
    }
}

public class EntityNullError<T> : IError
{
    public readonly string _message = 
        $"Provided entity of type '{typeof(T).Name}' is null";
    public string Message
    {
        get;
    }
}