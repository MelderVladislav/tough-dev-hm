namespace UberPopug.Infrastructure.EventBus.API;

public interface IEventBus
{
    void Publish<T>(T eventModel) where T : IEventModel;

    void Publish(string eventName, string eventJsonBody);
    
    void Subscribe<T, TH>() where T : IEventModel where TH : IEventHandler<T>;
}