using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Core.Events;

public interface IEventsStore
{
    Task SaveSentEvent<TEvent>(TEvent eventObject);

    Task MarkEventAsReceived(Guid eventId);

    Task<ICollection<SentEvent>> FindAllEventsWithError();

    Task<ICollection<SentEvent>> MarkExpiredErrorsForSupport();

    Task UpdateEventForRetry(Guid eventId);
}