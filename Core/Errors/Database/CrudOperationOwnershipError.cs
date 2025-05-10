namespace Core.Errors.Authentication;

public class CrudOperationOwnershipError : IError
{
    private readonly string _message = "Tried to access an entity that is owned by another user.";
    public string Message { get; }
}