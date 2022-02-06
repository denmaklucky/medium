namespace WebApi.Services.Results;

public class AddUserResult : ResultBase
{
    public AddUserResult(bool isSuccess, AddUserResultError error = AddUserResultError.None)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static AddUserResult Success()
        => new AddUserResult(true, AddUserResultError.None);

    public static AddUserResult Failed(AddUserResultError error)
        => new AddUserResult(false, error);

    public AddUserResultError Error { get; set; }
}

public enum AddUserResultError
{
    None,
    AlreadyExists,
    PasswordNotStrong
}