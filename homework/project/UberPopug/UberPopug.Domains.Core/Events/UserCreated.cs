using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Domains.Core.Events;

public class UserCreated : IEventModel
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string Role { get; set; }
}