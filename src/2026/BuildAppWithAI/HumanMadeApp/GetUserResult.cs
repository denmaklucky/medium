namespace HumanMadeApp;

public abstract record GetUserResult
{
    public sealed record NotFound : GetUserResult;

    public sealed record Success(string UserId, string Hash) : GetUserResult;
}
