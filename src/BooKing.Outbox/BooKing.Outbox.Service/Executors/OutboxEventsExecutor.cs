using BooKing.Generics.Outbox.Service;

namespace BooKing.Outbox.Service.Executors;
public class OutboxEventsExecutor : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOutboxEventService _outboxEventService;

    public OutboxEventsExecutor(ILogger<Worker> logger, 
                                IOutboxEventService outboxEventService)
    {
        _logger = logger;
        _outboxEventService = outboxEventService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BooKing.Outbox.Service - OutboxEventsExecutor starts at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("BooKing.Outbox.Service running at: {time}", DateTimeOffset.Now);

            var events = await _outboxEventService.ReadEvents();

            

            await Task.Delay(10000, stoppingToken);
        }
    }    
}
