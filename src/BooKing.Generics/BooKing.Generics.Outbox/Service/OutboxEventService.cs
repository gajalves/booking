using BooKing.Generics.Outbox.Entities;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Repository;
using Newtonsoft.Json;

namespace BooKing.Generics.Outbox.Service;
public class OutboxEventService : IOutboxEventService
{
    private readonly IOutboxReposity _reposity;

    public OutboxEventService(IOutboxReposity reposity)
    {
        _reposity = reposity;
    }

    public async Task AddEvent(string queue, Event json)
    {
        var obj = new OutboxIntegrationEvents(queue, json.GetType().Name, JsonConvert.SerializeObject(json));

        await _reposity.AddAsync(obj);
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
