using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Shared;

namespace BooKing.Generics.EventSourcing;
public class Event : IEvent
{    
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid AggregateId { get; set; }

    public Event(Guid aggregateId)
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTimeHelper.HoraBrasilia();
        AggregateId = aggregateId;
    }
}
