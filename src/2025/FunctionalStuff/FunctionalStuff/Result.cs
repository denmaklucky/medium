namespace FunctionalStuff;

public readonly struct Result
{
    private Result(bool isSuccess, string? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    
    public string? Error { get; }

    public static Result Success() => new(true);

    public static Result Failure(string error) => new(false, error);
}