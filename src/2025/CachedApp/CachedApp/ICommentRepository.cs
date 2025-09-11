namespace CachedApp;

public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> ListAsync();
}

internal sealed class NullCommentRepository : ICommentRepository
{
    public Task<IReadOnlyList<Comment>> ListAsync()
    {
        return Task.FromResult<IReadOnlyList<Comment>>(new List<Comment>());
    }
}