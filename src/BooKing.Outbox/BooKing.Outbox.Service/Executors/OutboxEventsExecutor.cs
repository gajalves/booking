using BooKing.Generics.Bus;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Outbox.Service;
using Microsoft.Extensions.Options;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BooKing.Outbox.Service - OutboxEventsExecutor starts at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("BooKing.Outbox.Service running at: {time}", DateTimeOffset.Now);

            var events = await _outboxEventService.ReadEvents();

            foreach (var ev in events)
            {                                
                var busOptions = EventBusOptions.Config(
                ev.Queue,
                ev.Queue,
                withDeadletter: true);
                
                _eventBus.Publish(ev.Payload, busOptions);

                await _outboxEventService.UpdateEventProcessedAt(ev);
            }

            await Task.Delay((int)TimeSpan.FromSeconds(_outboxOptions.IntervalInSeconds).TotalMilliseconds, stoppingToken);
        }
    }    
}
