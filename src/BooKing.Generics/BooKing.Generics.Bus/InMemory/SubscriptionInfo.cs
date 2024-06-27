using BooKing.Generics.Bus.Abstractions;

namespace BooKing.Generics.Bus.InMemory;
public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerType { get; }
        public Type EventType { get; }
        public string QueueName { get; set; }

        private SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        private SubscriptionInfo(bool isDynamic, Type handlerType, string queueName)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
            QueueName = queueName;
        }
        private SubscriptionInfo(bool isDynamic, Type handlerType, Type eventType, string queueName)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
            QueueName = queueName;
            EventType = eventType;
        }

        public static SubscriptionInfo Dynamic(Type handlerType)
        {
            return new SubscriptionInfo(true, handlerType);
        }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(false, handlerType);
        }

        public static SubscriptionInfo Typed(Type handlerType, Type eventType, string queueName)
        {
            return new SubscriptionInfo(false, handlerType, eventType, queueName);
        }
    }
}
