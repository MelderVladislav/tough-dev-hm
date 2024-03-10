using Microsoft.EntityFrameworkCore;

namespace UberPopug.Domains.Core.Infrastructure.Stores;

public interface IDatabaseContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}