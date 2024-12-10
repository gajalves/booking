using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.Infra.Serialization;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Outbox.Service;
using BooKing.Generics.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BooKing.Outbox.Service.Executors;
public class OutboxEventsExecutor : BackgroundService
{
    private readonly ILogger<OutboxEventsExecutor> _logger;
    private readonly IOutboxEventService _outboxEventService;
    private readonly IEventBus _eventBus;
    private readonly OutboxOptions _outboxOptions;

    public OutboxEventsExecutor(ILogger<OutboxEventsExecutor> logger,
                                IOutboxEventService outboxEventService,
                                IEventBus eventBus,
                                IOptions<OutboxOptions> outboxOptions)
    {
        _logger = logger;
        _outboxEventService = outboxEventService;
        _eventBus = eventBus;
        _outboxOptions = outboxOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BooKing.Outbox.Service - OutboxEventsExecutor starts at: {time}", DateTimeHelper.HoraBrasilia());
        while (!cancellationToken.IsCancellationRequested)
        {
            //_logger.LogInformation("BooKing.Outbox.Service running at: {time}", DateTimeHelper.HoraBrasilia());

            var events = await _outboxEventService.ReadEvents();

            foreach (var ev in events)
            {
                var @event = JsonConvert.DeserializeObject<Event>(ev.Payload, SerializerSettings.Instance);
                await _eventBus.PublishAsync<Event>(@event, cancellationToken);

                await _outboxEventService.UpdateEventProcessedAt(ev);
            }

            await Task.Delay((int)TimeSpan.FromSeconds(_outboxOptions.IntervalInSeconds).TotalMilliseconds, cancellationToken);
        }
    }
}
