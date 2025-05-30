using Core.Errors;

namespace Core;

public class Result
{
    public bool IsSuccess { get; init; }
    
    public bool IsError => !IsSuccess;
    
    public IError Error { get; init; } = null!;
    
    private readonly string? _overrideMessage;
    
    public string ErrorMessage
    {
        get => _overrideMessage ?? Error.Message;
        init => _overrideMessage = value;
    }
    
    public static Result Success() 
        => new() { IsSuccess = true};
    
    public static Result Failure(IError? error = null, string? overrideMessage = null)
        => new()
        {
            IsSuccess = false,
            Error      = error ?? new Error(),
            ErrorMessage = overrideMessage!
        };
}


public class Result<T> : Result
{
    public T? Value { get; set; }
    
    public static Result<T> Success(T data) 
        => new() { IsSuccess = true, Value = data };
    
    public static Result<T> Failure(IError error, string? overrideMessage = null)
        => new()
        {
            IsSuccess = false,
            Error      = error,
            ErrorMessage = overrideMessage!
        };
}