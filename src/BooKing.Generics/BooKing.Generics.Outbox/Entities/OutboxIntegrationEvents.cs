using BooKing.Generics.Domain;
using BooKing.Generics.Shared;

namespace BooKing.Generics.Outbox.Entities;
public class OutboxIntegrationEvents : Entity
{    
    public string EventType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Message { get; set; }

    public OutboxIntegrationEvents()
    {

    }

    public OutboxIntegrationEvents(string eventType, string payload)
    {        
        EventType = eventType;
        Payload = payload;
        CreatedAt = DateTimeHelper.HoraBrasilia();
    }

    public void SetProcessedAtToDateTimeNow()
    {
        this.ProcessedAt = DateTimeHelper.HoraBrasilia();
    }

    public void SetMessage(string message)
    {
        this.Message = message;
    }
}
