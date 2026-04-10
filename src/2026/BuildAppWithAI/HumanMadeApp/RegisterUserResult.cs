namespace HumanMadeApp;

public abstract record RegisterUserResult
{
    public sealed record UserAlreadyRegistered :  RegisterUserResult;

    public sealed record Success(Guid Id) :  RegisterUserResult;
}
