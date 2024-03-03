using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace UberPopug.Domains.TaskTracking.Stores;

public abstract class EntityStore<TEntity>
    where TEntity : class
{
    private readonly ITaskTrackingDatabaseContext dbContext;

    public EntityStore(ITaskTrackingDatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private DbSet<TEntity> EntitySet => dbContext.Set<TEntity>();

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        var addedEntry = await EntitySet.AddAsync(entity);

        await dbContext.SaveChangesAsync();

        return addedEntry.Entity;
    }

    public virtual async Task<TEntity?> Find(Guid id)
    {
        var entity = await EntitySet
            .FindAsync(id.ToString());

        return entity;
    }

    public virtual async Task<ICollection<TEntity>> GetBy(Expression<Func<TEntity, bool>> filterExpression)
    {
        var entitiesList = await EntitySet
            .Where(filterExpression)
            .ToListAsync();

        return entitiesList;
    }

    public virtual async Task<ICollection<TEntity>> GetBy<TKey>(Expression<Func<TEntity, bool>> filterExpression,
        int skip,
        int take,
        Expression<Func<TEntity, TKey>> orderExpression,
        bool ascending = true)
    {
        var query = EntitySet
            .Where(filterExpression)
            .Skip(skip)
            .Take(take);

        query = ascending
            ? query.OrderBy(orderExpression)
            : query.OrderByDescending(orderExpression);

        return await query.ToListAsync();
    }

    public virtual async Task<int> Count(Expression<Func<TEntity, bool>> filterExpression)
    {
        var entitiesList = await EntitySet
            .CountAsync(filterExpression);

        return entitiesList;
    }

    public virtual async Task<TEntity?> FirstOrDefaultBy(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await EntitySet.FirstOrDefaultAsync(filterExpression);
    }

    public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await EntitySet.AnyAsync(filterExpression);
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        var entry = EntitySet.Update(entity);

        await dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        var entityWithId = await EntitySet.FindAsync(id);
        if (entityWithId != null)
        {
            EntitySet.Remove(entityWithId);
            await dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public virtual async Task<TResult> Query<TResult>(Func<IQueryable<TEntity>, Task<TResult>> query)
    {
        var request = query(EntitySet);

        var entitiesList = await request;

        return entitiesList;
    }
}