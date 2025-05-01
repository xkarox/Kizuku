using Core.Entities;

namespace Core.Errors.Entities;

public class EntityAlreadyExistsError<T> : IError
{
    public readonly string _message = $"{typeof(T).Name} already exists.";

    public string Message => _message;
    
    public EntityAlreadyExistsError() {}

    public EntityAlreadyExistsError(string propertyName, string propertyValue)
    {
        _message = $"{typeof(T).Name} with {propertyName}: {propertyValue} not found";
    }
}