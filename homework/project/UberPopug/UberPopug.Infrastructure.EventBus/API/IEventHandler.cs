namespace UberPopug.Infrastructure.EventBus.API;

public interface IEventHandler<T> where T : IEventModel
{
    Task Handle(T eventModel);
}