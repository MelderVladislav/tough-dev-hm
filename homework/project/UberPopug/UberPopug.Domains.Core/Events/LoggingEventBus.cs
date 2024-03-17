using UberPopug.Domains.Core.Entities;
using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Domains.Core.Events;

public class LoggingEventBus: ILoggingEventBus
{
    private readonly IEventBus eventBus;
    private readonly IEventsStore eventsStore;

    public LoggingEventBus(IEventBus eventBus, IEventsStore eventsStore)
    {
        this.eventBus = eventBus;
        this.eventsStore = eventsStore;
    }

    public void Publish<T>(T eventModel) where T : IEventModel
    {
        eventsStore.SaveSentEvent(eventModel);
        eventBus.Publish(eventModel);
    }

    public void RetryPublish(Guid eventId, string eventName, string eventJsonBody)
    {
        eventsStore.UpdateEventForRetry(eventId);
        
        eventBus.Publish(eventName, eventJsonBody);
    }

    public void Subscribe<T, TH>() where T : IEventModel where TH : IEventHandler<T>
    {
        eventBus.Subscribe<T, TH>();
    }
}