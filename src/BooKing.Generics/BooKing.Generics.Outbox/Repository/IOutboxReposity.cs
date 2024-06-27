using BooKing.Generics.Outbox.Entities;

namespace BooKing.Generics.Outbox.Repository;
public interface IOutboxReposity
{
    Task AddAsync(OutboxIntegrationEvents outboxEvent);

    Task<List<OutboxIntegrationEvents>> ReadAsync();

    Task Commit();
}
