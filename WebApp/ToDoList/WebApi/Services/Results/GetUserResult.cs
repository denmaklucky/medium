using WebApi.Entities;

namespace WebApi.Services.Results;

public class GetUserResult : ResultBase
{
    public GetUserResult(bool isSuccess, User user = null, GetUserError error = GetUserError.None)
    {
        IsSuccess = isSuccess;
        Error = error;
        User = user;
    }

    public static GetUserResult Success(User user)
        => new GetUserResult(true, user);

    public static GetUserResult Failed()
        => new GetUserResult(false, error: GetUserError.NotFound);

    public GetUserError Error { get; set; }
    public User User { get; }
}

public enum GetUserError
{
    None,
    NotFound
}