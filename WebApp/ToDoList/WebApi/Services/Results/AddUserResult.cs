using WebApi.Entities;

namespace WebApi.Services.Results;

public class AddUserResult : ResultBase
{
    public AddUserResult(bool isSuccess,User user = null, AddUserResultError error = AddUserResultError.None)
    {
        IsSuccess = isSuccess;
        Error = error;
        User = user;
    }

    public static AddUserResult Success(User user)
        => new AddUserResult(true, user, AddUserResultError.None);

    public static AddUserResult Failed(AddUserResultError error)
        => new AddUserResult(false, error: error);

    public AddUserResultError Error { get; set; }
    
    public User User { get; set; }
}

public enum AddUserResultError
{
    None,
    AlreadyExists,
    PasswordNotStrong
}