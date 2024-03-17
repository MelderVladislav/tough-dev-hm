using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core;
using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Events;
using UberPopug.Infrastructure.EventBus.API;
using TaskStatus = UberPopug.Domains.Core.Entities.TaskStatus;

namespace UberPopug.BillingService.Handlers;

public class BalanceCalculatingHandler : IEventHandler<CalculateUsersBalanceEvent>
{
    private readonly ICoreDatabaseContext coreDatabaseContext;
    private readonly IEventsStore eventsStore;
    
    public BalanceCalculatingHandler(ICoreDatabaseContext coreDatabaseContext, IEventsStore eventsStore)
    {
        this.coreDatabaseContext = coreDatabaseContext;
        this.eventsStore = eventsStore;
    }
    
    public async Task Handle(CalculateUsersBalanceEvent eventModel)
    {
        var usersTasks = await coreDatabaseContext.WorkTasks.Where(task => task.Created.Date == DateTime.Now.AddDays(-1) && task.AssignedUser != null)
            .GroupBy(task => task.AssignedUser)
            .ToArrayAsync();

        foreach (var user in usersTasks)
        {
            var userBillPOperations = user.Select(task => new BillOperation
            {
                UserId = user.Key!.Value,
                Sum = task.Price,
                IsDebit = task.Status == TaskStatus.Done,
                OperationDate = DateTime.Now.AddDays(-1).Date
            }).ToArray();

            await coreDatabaseContext.BillsOperations.AddRangeAsync(userBillPOperations);
        }

        await eventsStore.MarkEventAsReceived(eventModel.EventId);

        await coreDatabaseContext.SaveChangesAsync();
    }
}