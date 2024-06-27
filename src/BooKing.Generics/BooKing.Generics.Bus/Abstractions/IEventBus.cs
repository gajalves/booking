namespace BooKing.Generics.Bus.Abstractions;
public interface IEventBus
{
    void Publish(IEvent @event, string queueName, string exchangeName);

    void Subscribe<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10, bool deadLetter = false)
            where E : IEvent
            where EH : IEventHandler<E>;
    
    void SubscribeInExchange<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10)
           where E : IEvent
           where EH : IEventHandler<E>;

    void Publish(IEvent @event, EventBusOptions options);

    void Publish(string serializedEvent, EventBusOptions options);

    void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
          where E : IEvent
          where EH : IEventHandler<E>;
}

