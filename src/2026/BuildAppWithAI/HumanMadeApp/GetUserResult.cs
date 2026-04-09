namespace HumanMadeApp;

public abstract record GetUserResult
{
    public sealed record NotFound : GetUserResult;

    public sealed record Success(Guid UserId, string Hash) : GetUserResult;
}
