namespace CSharpEfAndAspNet;

public sealed class Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    
    private Result(TError error)
    {
        _error = error;
    }

    private Result(TValue value)
    {
        _value = value;
    }

    public bool IsSuccess => _error == null;

    public TError? ErrorValue => _error ?? throw new InvalidOperationException();

    public TValue? Value => _value ?? throw new InvalidOperationException();

    public static Result<TValue, TError> Success(TValue value) => new(value);

    public static Result<TValue, TError> Error(TError error) => new(error);
}