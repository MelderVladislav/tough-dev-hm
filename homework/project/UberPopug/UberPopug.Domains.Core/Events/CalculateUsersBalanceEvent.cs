using UberPopug.Infrastructure.EventBus.API;

namespace UberPopug.Domains.Core.Events;

public class CalculateUsersBalanceEvent: IEventModel
{
    public Guid EventId { get; set; } = Guid.NewGuid();
}