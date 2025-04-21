namespace WebApi.Services.Results;

public class CompleteTaskResult : ResultBase
{
    public CompleteTaskResult(bool isSuccess, CompleteTaskError error = CompleteTaskError.None)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static CompleteTaskResult Success()
        => new CompleteTaskResult(true);

    public static CompleteTaskResult Failed()
        => new CompleteTaskResult(false, CompleteTaskError.NotFound);
    
    public CompleteTaskError Error { get; }
}

public enum CompleteTaskError
{
    None,
    NotFound
}