namespace BooKing.Generics.EventSourcing;
public class StoredEvent
{
    public StoredEvent(Guid id, 
                       string eventType, 
                       string data, 
                       DateTime createdAt)
    {
        Id = id;
        EventType = eventType;
        Data = data;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string EventType { get; private set; }
    public string Data { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
