using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWorkAndRepository;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    
    public IRepository<TEntity, TIdentifier> CreateRepository<TEntity, TIdentifier>(bool withTransaction = false)
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : notnull
    {
        if (withTransaction)
        {
            _transaction = context.Database.BeginTransaction();
        }

        return new Repository<TEntity, TIdentifier>(context);
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
        => _transaction?.CommitAsync(cancellationToken) ?? Task.CompletedTask;

    public Task RollbackAsync(CancellationToken cancellationToken = default)
        => _transaction?.RollbackAsync(cancellationToken) ?? Task.CompletedTask;

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction!.DisposeAsync();
        }

        await context.DisposeAsync();
    }
}