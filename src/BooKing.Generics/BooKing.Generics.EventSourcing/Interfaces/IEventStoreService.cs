using EventStore.Client;

namespace BooKing.Generics.EventSourcing.Interfaces;

public interface IEventStoreService
{
    EventStoreClient GetClient();    
}
