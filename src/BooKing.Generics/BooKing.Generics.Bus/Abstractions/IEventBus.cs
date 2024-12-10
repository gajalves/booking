namespace BooKing.Generics.Bus.Abstractions;
public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IEvent;
}

