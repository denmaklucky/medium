namespace CachedApp;

internal interface IGetComments
{
    Task<IReadOnlyList<Comment>> GetAsync(CancellationToken cancellationToken);
}

internal sealed class GetComments(ICommentRepository repository) : IGetComments
{
    public Task<IReadOnlyList<Comment>> GetAsync(CancellationToken _)
    {
        return repository.ListAsync();
    }
}