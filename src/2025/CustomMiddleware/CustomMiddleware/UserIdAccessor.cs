namespace CustomMiddleware;

public interface IUserIdAccessor
{
    Guid? UserId { get; set; }
}

internal sealed class UserIdAccessor : IUserIdAccessor
{
    public Guid? UserId { get; set; }
}