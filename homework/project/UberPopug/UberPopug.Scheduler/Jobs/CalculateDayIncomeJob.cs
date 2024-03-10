using UberPopug.Domains.Core.Events;
using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Scheduler.Jobs;

public class CalculateDayIncomeJob
{
    private readonly IEventBus eventBus;

    public CalculateDayIncomeJob(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public void CalculateDayIncome()
    {
        eventBus.Publish(new CalculateUsersBalanceEvent());
    }
}