using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.DataAccess;

public class UberPopugDbContext: DbContext, ICoreDatabaseContext
{
    public DbSet<WorkTask> WorkTasks { get; set; }
    public DbSet<BillOperation> BillsOperations { get; set; }
}