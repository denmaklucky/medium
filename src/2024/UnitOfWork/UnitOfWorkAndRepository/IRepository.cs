namespace UnitOfWorkAndRepository;

public interface IRepository<TEntity, in TIdentifier>
    where TEntity : class, IEntity<TIdentifier>
    where TIdentifier : notnull
{
    IQueryable<TEntity> Query();

    Task RegisterAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TIdentifier id, CancellationToken cancellationToken = default);
}