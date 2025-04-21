namespace UnitOfWorkAndRepository;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<TEntity, TIdentifier> CreateRepository<TEntity, TIdentifier>(bool withTransaction = false)
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : notnull;

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}