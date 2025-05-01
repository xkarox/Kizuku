using Core.Entities;

namespace Core.Errors.Entities;

public class EntityNotFoundError<T> : IError
{
    public readonly string _message;
    
    public string Message => _message;

    public EntityNotFoundError(IEntity entity)
    {
        _message = $"{typeof(T).Name} {entity} not found";
    }
    
    public EntityNotFoundError(Guid id)
    {
        _message = $"{typeof(T).Name} with Id: {id} not found";
    }
    
    public EntityNotFoundError(string propertyName, string propertyValue)
    {
        _message = $"{typeof(T).Name} with {propertyName}: {propertyValue} not found";
    }
}