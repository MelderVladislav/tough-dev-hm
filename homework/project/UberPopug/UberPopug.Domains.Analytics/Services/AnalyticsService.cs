using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core;
using UberPopug.Domains.Core.Entities;
using TaskStatus = UberPopug.Domains.Core.Entities.TaskStatus;

namespace UberPopug.Domains.Analytics.Services;

public class AnalyticsService
{
    private readonly ICoreDatabaseContext coreDatabaseContext;

    public AnalyticsService(ICoreDatabaseContext coreDatabaseContext)
    {
        this.coreDatabaseContext = coreDatabaseContext;
    }

    public async Task<ICollection<WorkTask>> GetMostExpensiveTasks(DateTime startDate, DateTime endDate)
    {
        return await coreDatabaseContext.WorkTasks
            .Where(task => task.Created > startDate && task.Created < endDate)
            .OrderByDescending(task => task.Price).ToArrayAsync();
    }
    
    public async Task<StatisticsResult> GetStatistics()
    {
        var notClosedCount = await coreDatabaseContext.WorkTasks
            .Where(task => task.Created.Date == DateTime.Now.Date && task.Status != TaskStatus.Done && task.AssignedUser != null)
            .GroupBy(task => task.AssignedUser)
            .CountAsync();

        var managementIncome = await coreDatabaseContext
            .WorkTasks
            .Where(task => task.Created.Date == DateTime.Now.Date && task.Status != TaskStatus.Done &&
                           task.AssignedUser != null)
            .SumAsync(task => task.Price);

        return new StatisticsResult(notClosedCount, managementIncome);
    }
}

public record StatisticsResult(int NegativeBalancesCount, decimal ManagementIncome);