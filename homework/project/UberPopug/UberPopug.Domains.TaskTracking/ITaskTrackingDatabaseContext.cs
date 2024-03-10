using Microsoft.EntityFrameworkCore;

namespace UberPopug.Domains.TaskTracking;

public interface ITaskTrackingDatabaseContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}