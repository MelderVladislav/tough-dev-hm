using UberPopug.Domains.Core.Events;

namespace UberPopug.Scheduler.Jobs;

public class CalculateDayIncomeJob
{
    private readonly ILoggingEventBus eventBus;

    public CalculateDayIncomeJob(ILoggingEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public void CalculateDayIncome()
    {
        eventBus.Publish(new CalculateUsersBalanceEvent());
    }
}