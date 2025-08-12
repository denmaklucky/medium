namespace ProblemDetailsApp;

public sealed class ErrorResponse(int status, string message)
{
    public int Status { get; } = status;
    
    public string Message { get; } = message;
}