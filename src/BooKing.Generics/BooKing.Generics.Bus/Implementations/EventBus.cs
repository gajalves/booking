using BooKing.Generics.Bus.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BooKing.Generics.Bus.Implementations;
public class EventBus : IEventBus
{
    private readonly IPublishEndpoint _bus;
    private readonly ILogger<EventBus> _logger;

    public EventBus(IPublishEndpoint bus, ILogger<EventBus> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent
    {
        var ev = JsonConvert.SerializeObject(@event);
        try
        {
            _logger.LogInformation("Publishing event: {EventType} with Event: {Event}", @event.GetType().Name, ev);

            await _bus.Publish(@event, cancellationToken);

            _logger.LogInformation("Successfully published event: {EventType} with Event: {Event}", @event.GetType().Name, ev);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event: {EventType} with Event: {Event}", @event.GetType().Name, ev);
            throw;
        }
    }
}
