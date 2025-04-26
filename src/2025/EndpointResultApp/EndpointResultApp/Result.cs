namespace EndpointResultApp;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    
    private Result(TValue value)
    {
        _value = value;
        IsSuccess = true;
    }

    private Result(TError error)
    {
        _error = error;
        IsSuccess = false;
    }
    
    public bool IsSuccess { get; }
    
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Result is not successful.");
    
    public TError Error => !IsSuccess ? _error! : throw new InvalidOperationException("Result is successful.");

    public static Result<TValue, TError> Success(TValue value) => new(value);

    public static Result<TValue, TError> Failure(TError error) => new(error);
}