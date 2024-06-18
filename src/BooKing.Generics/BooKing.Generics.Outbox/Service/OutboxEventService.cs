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
}
