using BooKing.Generics.Domain;

namespace BooKing.Generics.Outbox.Entities;
public class OutboxIntegrationEvents : Entity
{
    public string Queue { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    public OutboxIntegrationEvents()
    {

    }

    public OutboxIntegrationEvents(string queue, string eventType, string payload)
    {
        Queue = queue;
        EventType = eventType;
        Payload = payload;
        CreatedAt = DateTime.Now;
    }
}
