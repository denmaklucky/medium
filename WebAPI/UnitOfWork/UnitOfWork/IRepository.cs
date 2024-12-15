namespace UnitOfWork;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : class, IEntity<TIdentifier>
    where TIdentifier : notnull
{
    IQueryable<TEntity> Query();

    Task<TIdentifier> RegisterAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TIdentifier id, CancellationToken cancellationToken = default);
}