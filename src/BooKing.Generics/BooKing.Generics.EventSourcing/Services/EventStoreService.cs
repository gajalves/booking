using BooKing.Generics.EventSourcing.Interfaces;
using EventStore.Client;
using Microsoft.Extensions.Configuration;

namespace BooKing.Generics.EventSourcing.Services;
public class EventStoreService : IEventStoreService
{
    private readonly EventStoreClient _client;

    public EventStoreService(IConfiguration configuration)
    {        
        var settings = EventStoreClientSettings.Create(configuration.GetConnectionString("EventStoreConnection"));
        var client = new EventStoreClient(settings);
        _client = client;
    }

    public EventStoreClient GetClient()
    {
        return _client;
    }
}
