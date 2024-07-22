using BooKing.Generics.Outbox.Entities;

namespace BooKing.Generics.Outbox.Repository;
public interface IOutboxReposity
{
    Task<OutboxIntegrationEvents> GetByEventIdAsync(Guid id);

    Task AddAsync(OutboxIntegrationEvents outboxEvent);

    Task<List<OutboxIntegrationEvents>> ReadAsync();

    Task Commit();
}
