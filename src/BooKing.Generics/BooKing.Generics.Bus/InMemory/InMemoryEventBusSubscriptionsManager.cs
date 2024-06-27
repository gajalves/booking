using BooKing.Generics.Bus.Abstractions;

namespace BooKing.Generics.Bus.InMemory;
public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public event EventHandler<string> OnEventRemoved;

    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => !_handlers.Keys.Any();
    public void Clear() => _handlers.Clear();

    public void AddSubscription<T, TH>(string queue)
        where T : IEvent
        where TH : IEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        DoAddSubscriptionByQueue(typeof(TH), typeof(T), queue, isDynamic: false);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }

    public string GetEventKey<T>()
    {
        return typeof(T).Name;
    }

    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

    public Type GetEventTypeByNameQueue(string queueName)
    {
        var eventName = _handlers.FirstOrDefault(c => c.Key == queueName);

        var result = eventName.Value.FirstOrDefault().EventType;
        return result;
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

    public bool HasSubscriptionsForEvent<T>() where T : IEvent
    {
        throw new NotImplementedException();
    }

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public void RemoveSubscription<T, TH>()
        where T : IEvent
        where TH : IEventHandler<T>
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        DoRemoveHandler(eventName, handlerToRemove);
    }

    private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, new List<SubscriptionInfo>());
        }

        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
        {
            throw new ArgumentException(
                $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        }

        if (isDynamic)
        {
            _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
        }
        else
        {
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
        }
    }

    private void DoAddSubscriptionByQueue(Type handlerType, Type eventType, string queue, bool isDynamic)
    {
        if (!HasSubscriptionsForEvent(queue))
        {
            _handlers.Add(queue, new List<SubscriptionInfo>());
        }

        if (_handlers[queue].Any(s => s.HandlerType == handlerType))
        {
            throw new ArgumentException(
                $"Handler Type {handlerType.Name} already registered for '{queue}'", nameof(handlerType));
        }

        if (isDynamic)
        {
            _handlers[queue].Add(SubscriptionInfo.Dynamic(handlerType));
        }
        else
        {
            _handlers[queue].Add(SubscriptionInfo.Typed(handlerType, eventType, queue));
        }
    }

    private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
      where T : IEvent
      where TH : IEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if (subsToRemove != null)
        {
            _handlers[eventName].Remove(subsToRemove);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }
                RaiseOnEventRemoved(eventName);
            }
        }
    }

    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        if (handler != null)
        {
            OnEventRemoved(this, eventName);
        }
    }
}

