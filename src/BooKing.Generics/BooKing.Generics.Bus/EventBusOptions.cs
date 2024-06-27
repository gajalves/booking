namespace BooKing.Generics.Bus;
public class EventBusOptions
{
    public string ExchangeName { get; private set; }
    public string QueueName { get; private set; }
    public ushort? PrefetchCount { get; private set; } = 10;
    public bool WithDeadletter { get; private set; } = false;
    public bool WithDeadletterTimeToLive { get; private set; } = false;
    public ushort TimeToLiveInMinutes { get; private set; } = 5;

    public EventBusOptions()
    {
        
    }

    public static EventBusOptions Config(string exchangeName, string queueName, bool withDeadletter, ushort? prefetchCount = 10)
    {
        return new EventBusOptions
        {
            ExchangeName = exchangeName,
            QueueName = queueName,
            WithDeadletter = withDeadletter,
            PrefetchCount = prefetchCount
        };
    }
}
