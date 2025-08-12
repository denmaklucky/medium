namespace ProblemDetailsApp;

public abstract class Result;

public sealed class Success : Result;

public sealed class Data(object value) : Result
{
    public object Value { get; } = value;
}

public abstract class Error : Result
{
    public string? Description { get; set; }
}

public sealed class InvalidInput : Error;

public sealed class ResourceMissing : Error;

public sealed class AccessDenied : Error;