using Microsoft.EntityFrameworkCore;

namespace UnitOfWork;

public sealed class Repository<TEntity, TIdentifier>(AppDbContext context) : IRepository<TEntity, TIdentifier>
    where TEntity : class, IEntity<TIdentifier>
    where TIdentifier : notnull
{
    public IQueryable<TEntity> Query() => context.Set<TEntity>();

    public async Task<TIdentifier> RegisterAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity.Id.Equals(default))
            {
                await context.Set<TEntity>().AddAsync(entity, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            foreach (var entry in exception.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues == null)
                {
                    // The entity was deleted
                    entry.State = EntityState.Detached;
                    continue;
                }

                // Resolve by overwriting the database with current values
                entry.OriginalValues.SetValues(databaseValues);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        return entity.Id!;
    }

    public async Task DeleteAsync(TIdentifier id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await context.Set<TEntity>().FindAsync([id], cancellationToken);

            if (entity == null)
            {
                return;
            }

            context.Set<TEntity>().Remove(entity);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            foreach (var entry in exception.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues == null)
                {
                    // The entity was deleted
                    entry.State = EntityState.Detached;
                    return;
                }

                // Resolve by overwriting the database with current values
                entry.OriginalValues.SetValues(databaseValues);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}