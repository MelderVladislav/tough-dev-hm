namespace UberPopug.Domains.Core.Entities;

public class SentEvent
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public string EventClassName { get; set; }

    public DateTime CreatedDate { get; set; }

    public EventStatus EventStatus { get; set; }

    public string JsonBody { get; set; }

    public int AttemptsToSend { get; set; } = 1;
}

public enum EventStatus
{
    Sent,
    Received,
    HandlingError,
    ForSupport,
}