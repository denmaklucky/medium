namespace DisciminatedUnion;

public sealed class Result
{
    public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

    public string? Error { get; set; }

    public ErrorType? ErrorType { get; set; }
}