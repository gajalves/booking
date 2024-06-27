using BooKing.Generics.Outbox.Entities;
using BooKing.Generics.Outbox.Events;

namespace BooKing.Generics.Outbox.Service;
public interface IOutboxEventService
{
    Task AddEvent(string queue, Event json);
    Task<List<OutboxIntegrationEvents>> ReadEvents();
    Task UpdateEventProcessedAt(OutboxIntegrationEvents ev);
}
