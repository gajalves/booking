using BooKing.Generics.EventSourcing;
using BooKing.Generics.Infra.Serialization;
using BooKing.Generics.Outbox.Entities;
using BooKing.Generics.Outbox.Repository;
using Newtonsoft.Json;

namespace BooKing.Generics.Outbox.Service;
public class OutboxEventService : IOutboxEventService
{
    private readonly IOutboxReposity _repository;
    private readonly IEventSourcingRepository _eventSourcingRepository;

    public OutboxEventService(IOutboxReposity reposity, 
                              IEventSourcingRepository eventSourcingRepository)
    {
        _repository = reposity;
        _eventSourcingRepository = eventSourcingRepository;
    }

    public async Task AddEvent(Event @event)
    {
        var obj = new OutboxIntegrationEvents(@event.GetType().Name, JsonConvert.SerializeObject(@event, SerializerSettings.Instance));

        await _repository.AddAsync(obj);
        await _eventSourcingRepository.SaveEvent(@event);
    }

    public async Task AddEventAlreadyProcessed(Event @event, string processedBy)
    {
        var obj = new OutboxIntegrationEvents(@event.GetType().Name, JsonConvert.SerializeObject(@event));
        obj.SetMessage($"Event processed by: {processedBy}");
        obj.SetProcessedAtToDateTimeNow();

        await _repository.AddAsync(obj);
        await _eventSourcingRepository.SaveEvent(@event);
    }

    public async Task<List<OutboxIntegrationEvents>> ReadEvents()
    {
       return await _repository.ReadAsync();
    }

    public async Task SetMessage(Guid eventId, string message)
    {
        var ev = await _repository.GetByEventIdAsync(eventId);                

        ev.SetMessage(message);        

        await _repository.Commit();
    }

    public async Task UpdateEventProcessedAt(OutboxIntegrationEvents ev)
    {
        ev.SetProcessedAtToDateTimeNow();

        await _repository.Commit();
    }
}
