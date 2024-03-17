using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Core;

public interface ICoreDatabaseContext
{
    DbSet<WorkTask> WorkTasks { get; set; }
    
    DbSet<BillOperation> BillsOperations { get; set; }

    DbSet<SentEvent> SentEvents { get; set; }

    Task<ICollection<Guid>> GetEngineersId();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}