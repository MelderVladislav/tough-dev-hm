using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Core;

public interface ICoreDatabaseContext
{
    public DbSet<WorkTask> WorkTasks { get; set; }
}