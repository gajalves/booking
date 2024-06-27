using BooKing.Generics.Bus.Abstractions;

namespace BooKing.Generics.Outbox.Events;
public class Event : IEvent
{    
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public Event()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}
