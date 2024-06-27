using Autofac;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.RabbitMQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace BooKing.Generics.Bus.Implementations;
public class EventBus : IEventBus, IDisposable
{
    private readonly IRabbitMQConnection _rabbitConnection;
    private readonly ILogger<EventBus> _logger;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly ILifetimeScope _autofac;
    private IModel _consumerChannel;

    private const string AUTOFAC_SCOPE_NAME = "BooKing-Generics-Bus";

    public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection)
    {
        _subsManager = subsManager;
        _rabbitConnection = rabbitConnection;
    }

    public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection, ILogger<EventBus> logger)
    {
        _subsManager = subsManager;
        _rabbitConnection = rabbitConnection;
        _logger = logger;
    }

    public EventBus(IEventBusSubscriptionsManager subsManager, IRabbitMQConnection rabbitConnection, ILogger<EventBus> logger, ILifetimeScope autofac) : this(subsManager, rabbitConnection, logger)
    {
        _autofac = autofac;
    }

    public void Publish(IEvent @event, EventBusOptions options)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();

        var policy = CreatePolicyRetry();

        using (var channel = _rabbitConnection.CreateModel())
        {
            var arguments = new Dictionary<string, Object>();
            bool isDeadletter = options.WithDeadletter || options.WithDeadletterTimeToLive;

            if (isDeadletter)
                arguments = CreateDeadLetterQueue(channel, options);

            DeclareExchangeDirect(channel, options.ExchangeName);
            DeclareQueue(channel, options.QueueName, arguments);
            BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);

            policy.Execute((Action)(() =>
            {
                PublishMessage(channel, @event, options.QueueName, options.ExchangeName);
            }));
        }
    }

    public void Publish(string serializedEvent, EventBusOptions options)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();

        var policy = CreatePolicyRetry();

        using (var channel = _rabbitConnection.CreateModel())
        {
            var arguments = new Dictionary<string, Object>();
            bool isDeadletter = options.WithDeadletter || options.WithDeadletterTimeToLive;

            if (isDeadletter)
                arguments = CreateDeadLetterQueue(channel, options);

            DeclareExchangeDirect(channel, options.ExchangeName);
            DeclareQueue(channel, options.QueueName, arguments);
            BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);

            policy.Execute((Action)(() =>
            {
                PublishMessage(channel, serializedEvent, options.QueueName, options.ExchangeName);
            }));
        }
    }

    public void Publish(IEvent @event, string queueName, string exchangeName = "")
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();

        var policy = CreatePolicyRetry();

        using (var channel = _rabbitConnection.CreateModel())
        {
            DeclareExchangeDirect(channel, exchangeName);
            DeclareQueue(channel, queueName, null);
            BindQueue(channel, exchangeName, queueName, queueName);

            policy.Execute((Action)(() =>
            {
                PublishMessage(channel, @event, queueName, exchangeName);
            }));
        }
    }

    public void Subscribe<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10, bool deadLetter = false)
        where E : IEvent
        where EH : IEventHandler<E>
    {
        _subsManager.AddSubscription<E, EH>(queueName);

        DoInternalSubscriptionByQueue(queueName, exchangeName, prefetchCount, deadLetter);
    }

    public void SubscribeDynamic<E, EH>(string eventName)
        where E : IEvent
        where EH : IEventHandler<E>
    {
        throw new NotImplementedException();
    }

    public void SubscribeInExchange<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10)
        where E : IEvent
        where EH : IEventHandler<E>
    {
        throw new NotImplementedException();
    }

    public void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
        where E : IEvent
        where EH : IEventHandler<E>
    {
        //
    }

    public void Dispose()
    {
        //throw new NotImplementedException();
    }

    private static RetryPolicy CreatePolicyRetry()
    {
        return RetryPolicy.Handle<BrokerUnreachableException>()
                          .Or<SocketException>()
                          .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
    }

    private static void DeclareExchangeDirect(IModel channel, string exchangeName)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    }

    private static void DeclareQueue(IModel channel, string queueName, Dictionary<string, Object> arguments)
    {
        channel.QueueDeclare(queueName, true, false, false, arguments);
    }

    private static void BindQueue(IModel channel, string exchangeName, string queueName, string routingKey)
    {
        channel.QueueBind(queueName, exchangeName, routingKey, null);
    }

    private static void PublishMessage(IModel channel, IEvent @event, string queueName, string exchangeName)
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;

        var message = JsonConvert.SerializeObject(@event);
        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchangeName, queueName, true, properties, messageBody);
    }

    private static void PublishMessage(IModel channel, string serializedEvent, string queueName, string exchangeName)
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;

        var message = serializedEvent;
        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchangeName, queueName, true, properties, messageBody);
    }

    private Dictionary<string, object> CreateDeadLetterQueue(IModel channel, EventBusOptions options)
    {
        string deadLetterQueueName = $"{options.QueueName}.dead-letter";
        string deadLetterExchangeName = "dead-letter-exchange";

        var argumentsDeadletter = new Dictionary<string, Object>();

        if (options.WithDeadletterTimeToLive)
        {
            argumentsDeadletter.Add("x-dead-letter-exchange", options.ExchangeName);
            argumentsDeadletter.Add("x-dead-letter-routing-key", options.QueueName);
            argumentsDeadletter.Add("x-message-ttl", (int)TimeSpan.FromMinutes(options.TimeToLiveInMinutes).TotalMilliseconds);
        }

        DeclareExchangeDirect(channel, deadLetterExchangeName);
        DeclareQueue(channel, deadLetterQueueName, argumentsDeadletter);
        BindQueue(channel, deadLetterExchangeName, deadLetterQueueName, deadLetterQueueName);

        var arguments = new Dictionary<string, Object>()
            {
                {"x-dead-letter-exchange",  deadLetterExchangeName },
                {"x-dead-letter-routing-key", deadLetterQueueName },
            };

        return arguments;
    }

    private static void DeclareExchangeFanOut(IModel channel, string exchangeName)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);
    }

    private void DoInternalSubscriptionByQueue(string queueName, string exchangeName, ushort? prefetchCount, bool deadLetter)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(queueName);

        if (!containsKey)
        {
            if (!_rabbitConnection.IsConnected)
            {
                _rabbitConnection.TryConnect();
            }

            using (var channel = _rabbitConnection.CreateModel())
            {
                BindQueue(channel, exchangeName, queueName, queueName);
            }
        }

        CreateRabbitMQListener(queueName, exchangeName, prefetchCount, deadLetter);
    }

    private IModel CreateRabbitMQListener(string queueName, string exchangeName, ushort? prefetchCount, bool deadLetter = false)
    {
        if (!_rabbitConnection.IsConnected)
        {
            _rabbitConnection.TryConnect();
        }

        var channel = _rabbitConnection.CreateModel();

        if (prefetchCount != null)
            channel.BasicQos(prefetchSize: 0, prefetchCount: prefetchCount.Value, global: false);

        var arguments = new Dictionary<string, Object>();

        if (deadLetter)
            arguments = CreateDeadLetterQueue(
                channel,
                EventBusOptions.Config(exchangeName, queueName, deadLetter, prefetchCount));

        DeclareExchangeDirect(channel, exchangeName);
        DeclareQueue(channel, queueName, arguments);
        BindQueue(channel, exchangeName, queueName, queueName);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var eventName = ea.RoutingKey;
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.Span);

            var success = await ProcessEvent(eventName, message);

            if (success)
                channel.BasicAck(ea.DeliveryTag, multiple: false);

            if (!success && deadLetter)
            {
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                _logger.LogCritical($"Queue: {queueName} - Message: {message}");
            }
        };

        DeclareConsumer(channel, consumer, queueName);

        channel.CallbackException += (sender, ea) =>
        {
            _consumerChannel.Dispose();
            _consumerChannel = CreateRabbitMQListener(queueName, exchangeName, prefetchCount);
        };

        return channel;
    }

    private async Task<bool> ProcessEvent(string eventName, string message)
    {
        try
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);

                        if (handler == null)
                            continue;

                        var eventType = _subsManager.GetEventTypeByNameQueue(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        return await (Task<bool>)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (_logger != null)
            {
                _logger.LogCritical(ex, "Erro ao processar evento na camada Generics.Bus");
            }

            return false;
        }

        return false;
    }

    private static void DeclareConsumer(IModel channel, EventingBasicConsumer consumer, string queueName)
    {
        channel.BasicConsume(queueName, false, consumer);
    }
}
