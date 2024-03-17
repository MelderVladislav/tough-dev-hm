using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Events;

namespace UberPopug.Scheduler.Jobs;

public class EventsErrorsHandlingJob
{
    private readonly ILoggingEventBus eventBus;
    private readonly IEventsStore eventsStore;

    public EventsErrorsHandlingJob(ILoggingEventBus eventBus, IEventsStore eventsStore)
    {
        this.eventBus = eventBus;
        this.eventsStore = eventsStore;
    }

    public async Task RetryToSendEvents()
    {
        var eventsWithError = await eventsStore.FindAllEventsWithError();

        foreach (var e in eventsWithError)
        {
            eventBus.RetryPublish(e.Id, e.EventClassName, e.JsonBody);
        }
    }
    
    public async Task MarkExpiredEventsForSupport()
    { 
        await eventsStore.MarkExpiredErrorsForSupport();
    }
}