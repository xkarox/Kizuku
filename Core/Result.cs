namespace Core;

public class Result
{
    public bool IsSuccess { get; init; }
    
    public bool IsFailure => !IsSuccess;
    
    public string? FailureMessage { get; set; }
    
    public static Result Success() 
        => new() { IsSuccess = true};
    
    public static Result Failure(string? message = null)
        => new() { IsSuccess = false, FailureMessage = message };
}


public class Result<T> : Result
{
    public T? Data { get; set; }
    
    public static Result<T> Success(T data) 
        => new() { IsSuccess = true, Data = data };
    
    public static Result<T> Failure(string? message = null)
        => new() { IsSuccess = false, FailureMessage = message };
}