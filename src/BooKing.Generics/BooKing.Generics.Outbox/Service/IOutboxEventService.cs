using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Entities;
using BooKing.Generics.Outbox.Events;

namespace BooKing.Generics.Outbox.Service;
public interface IOutboxEventService
{
    Task AddEvent(string queue, Event json);
    Task AddEventAlreadyProcessed(string queue, Event json, string processedBy);
    Task<List<OutboxIntegrationEvents>> ReadEvents();
    Task UpdateEventProcessedAt(OutboxIntegrationEvents ev);
    Task SetMessage(Guid eventId, string message);
}
