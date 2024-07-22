using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Shared;

namespace BooKing.Generics.Outbox.Events;
public class Event : IEvent
{    
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Event()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTimeHelper.HoraBrasilia();
    }
}
