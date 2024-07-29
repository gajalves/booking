using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Entities;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Repository;
using Newtonsoft.Json;

namespace BooKing.Generics.Outbox.Service;
public class OutboxEventService : IOutboxEventService
{
    private readonly IOutboxReposity _reposity;
    private readonly IEventSourcingRepository _eventSourcingRepository;

    public OutboxEventService(IOutboxReposity reposity, 
                              IEventSourcingRepository eventSourcingRepository)
    {
        _reposity = reposity;
        _eventSourcingRepository = eventSourcingRepository;
    }

    public async Task AddEvent(string queue, Event @event)
    {
        var obj = new OutboxIntegrationEvents(queue, @event.GetType().Name, JsonConvert.SerializeObject(@event));

        await _reposity.AddAsync(obj);
        await _eventSourcingRepository.SaveEvent(@event);
    }

    public async Task AddEventAlreadyProcessed(string queue, Event @event, string processedBy)
    {
        var obj = new OutboxIntegrationEvents(queue, @event.GetType().Name, JsonConvert.SerializeObject(@event));
        obj.SetMessage($"Event processed by: {processedBy}");
        obj.SetProcessedAtToDateTimeNow();

        await _reposity.AddAsync(obj);
        await _eventSourcingRepository.SaveEvent(@event);
    }

    public async Task<List<OutboxIntegrationEvents>> ReadEvents()
    {
       return await _reposity.ReadAsync();
    }

    public async Task SetMessage(Guid eventId, string message)
    {
        var ev = await _reposity.GetByEventIdAsync(eventId);                

        ev.SetMessage(message);        

        await _reposity.Commit();
    }

    public async Task UpdateEventProcessedAt(OutboxIntegrationEvents ev)
    {
        ev.SetProcessedAtToDateTimeNow();

        await _reposity.Commit();
    }
}
