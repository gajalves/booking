namespace BooKing.Generics.EventSourcing;
public interface IEventSourcingRepository
{
    Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event;
    Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId);
}
