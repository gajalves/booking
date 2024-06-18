namespace BooKing.Outbox.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("BooKing.Outbox.Service running at: {time}", DateTimeOffset.Now);
            await Task.Delay(20000, stoppingToken);
        }
    }
}
