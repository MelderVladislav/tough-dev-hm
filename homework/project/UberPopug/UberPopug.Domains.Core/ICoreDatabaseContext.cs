using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Core;

public interface ICoreDatabaseContext
{
    public DbSet<WorkTask> WorkTasks { get; set; }
    
    public DbSet<BillOperation> BillsOperations { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}