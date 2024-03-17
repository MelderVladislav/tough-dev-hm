using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Domains.Core.Events;

public interface ILoggingEventBus
{
    void Publish<T>(T eventModel) where T : IEventModel;

    void RetryPublish(Guid eventId, string eventName, string eventJsonBody);
    
    void Subscribe<T, TH>() where T : IEventModel where TH : IEventHandler<T>;
}