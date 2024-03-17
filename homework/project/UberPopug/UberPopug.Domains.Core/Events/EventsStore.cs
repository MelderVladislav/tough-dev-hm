using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Core.Events;

public class EventsStore : IEventsStore
{
    private readonly ICoreDatabaseContext coreDatabaseContext;

    public EventsStore(ICoreDatabaseContext coreDatabaseContext)
    {
        this.coreDatabaseContext = coreDatabaseContext;
    }
    
    public async Task SaveSentEvent<TEvent>(TEvent eventObject)
    {
        var eventToSave = new SentEvent
        {
            EventClassName = nameof(TEvent),
            JsonBody = JsonConvert.SerializeObject(eventObject),
            EventStatus = EventStatus.Sent,
            CreatedDate = DateTime.UtcNow,
        };

        coreDatabaseContext.SentEvents.Add(eventToSave);
        
        await coreDatabaseContext.SaveChangesAsync();
    }
    
    public async Task UpdateEventForRetry(Guid eventId)
    {
        var eventEntity = await coreDatabaseContext.SentEvents.FindAsync(eventId);

        eventEntity.EventStatus = EventStatus.Sent;
        eventEntity.AttemptsToSend++;
        
        await coreDatabaseContext.SaveChangesAsync();
    }
    
    public async Task MarkEventAsReceived(Guid eventId)
    {
        var eventEntity = await coreDatabaseContext.SentEvents.FindAsync(eventId);

        eventEntity.EventStatus = EventStatus.Received;
        
        await coreDatabaseContext.SaveChangesAsync();
    }
    
    public async Task<ICollection<SentEvent>> FindAllEventsWithError()
    {
        return await coreDatabaseContext.SentEvents
            .Where(c => c.EventStatus == EventStatus.HandlingError && c.CreatedDate > DateTime.UtcNow.AddHours(-1))
            .ToArrayAsync();
    }
    
    public async Task<ICollection<SentEvent>> MarkExpiredErrorsForSupport()
    {
        return await coreDatabaseContext.SentEvents
            .Where(c => (c.EventStatus == EventStatus.HandlingError || c.EventStatus == EventStatus.Sent) && c.CreatedDate < DateTime.UtcNow.AddHours(-1))
            .ToArrayAsync();
    }
}