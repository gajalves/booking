using BooKing.Generics.EventSourcing.Interfaces;
using EventStore.Client;
using Newtonsoft.Json;
using System.Text;

namespace BooKing.Generics.EventSourcing.Repository;
public class EventSourcingRepository : IEventSourcingRepository
{
    private readonly IEventStoreService _eventStoreService;

    public EventSourcingRepository(IEventStoreService eventStoreService)
    {
        _eventStoreService = eventStoreService;
    }

    public async Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event
    {
        await _eventStoreService.GetClient().AppendToStreamAsync(
            @event.AggregateId.ToString(),
            StreamState.Any,
            InstantiateEvent(@event));
    }

    public async Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId)
    {
        var evts = await _eventStoreService
                            .GetClient()
                            .ReadStreamAsync(Direction.Forwards, aggregateId.ToString(), 0, 500)
                            .ToListAsync();

        var eventList = new List<StoredEvent>();

        foreach (var ev in evts)
        {
            var dataEncoded = Encoding.UTF8.GetString(ev.Event.Data.ToArray());
            var jsonData = JsonConvert.DeserializeObject<Event>(dataEncoded);

            var @event = new StoredEvent(
                    new Guid(ev.Event.EventId.ToString()),
                    ev.Event.EventType,
                    dataEncoded,
                    jsonData.CreatedAt);

            eventList.Add(@event);
        }

        return eventList.OrderBy(x => x.CreatedAt);
    }

    private static IEnumerable<EventData> InstantiateEvent<TEvent>(TEvent @event) where TEvent : Event
    {
        yield return new EventData(
            Uuid.NewUuid(),
            @event.GetType().Name,            
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)));
    }
}
