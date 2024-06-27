using static BooKing.Generics.Bus.InMemory.InMemoryEventBusSubscriptionsManager;

namespace BooKing.Generics.Bus.Abstractions;
public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }
    event EventHandler<string> OnEventRemoved;
    void AddSubscription<T, TH>(string queue) where T : IEvent where TH : IEventHandler<T>;
    void RemoveSubscription<T, TH>() where TH : IEventHandler<T> where T : IEvent;
    bool HasSubscriptionsForEvent<T>() where T : IEvent;
    bool HasSubscriptionsForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    Type GetEventTypeByNameQueue(string queueName);
    void Clear();
    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IEvent;
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
    string GetEventKey<T>();
}
