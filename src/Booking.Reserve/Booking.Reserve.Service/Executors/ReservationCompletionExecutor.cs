using BooKing.Reserve.Service.Configurations;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Generics.Shared;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace BooKing.Reserve.Service.Executors;
public class ReservationCompletionExecutor : BackgroundService
{
    private readonly ILogger<ReservationCompletionExecutor> _logger;
    private readonly ExecutorOptions _outboxOptions;
    private readonly IReservationRepository _reservationRepository;
    private readonly IOutboxEventService _outboxEventService;

    public ReservationCompletionExecutor(ILogger<ReservationCompletionExecutor> logger,
                                         IOptions<ExecutorOptions> outboxOptions,
                                         IReservationRepository reservationRepository,
                                         IOutboxEventService outboxEventService)
    {
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
        _reservationRepository = reservationRepository;
        _outboxEventService = outboxEventService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking.Reserve.Service - ReservationCompletionExecutor starts at: {time}", DateTimeOffset.Now);
        
        var secondsToWaitFirstTime = CalculateFirstTimeStart(_outboxOptions.ReservationCompletionExecutorStartTime);
        
        await Task.Delay(TimeSpan.FromSeconds(secondsToWaitFirstTime), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Booking.Reserve.Service - ReservationCompletionExecutor running at: {time}", DateTimeOffset.Now);

            var yesterday = DateTimeHelper.HoraBrasilia().Date.AddDays(-1);
            var reservationsToComplete = await _reservationRepository.GetReservationsByStatusAndEndDateAsync(ReservationStatus.Reserved, yesterday.Date);

            foreach (var reservation in reservationsToComplete)
            {
                try
                {
                    reservation.SetCompletedStatus();
                    _reservationRepository.Update(reservation);

                    var completedEvent = new ReservationCompletedEvent(reservation.Id);
                    await _outboxEventService.AddEventAlreadyProcessed(QueueMapping.NoQueueNeeded, completedEvent, "ReservationCompletionExecutor");

                    _logger.LogInformation($"[ReservationCompletionExecutor] Reservation {reservation.Id} status updated to Completed.");                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[ReservationCompletionExecutor] Error updating reservation {reservation.Id} to Completed.");
                }
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private int CalculateFirstTimeStart(string firstExecution)
    {
        var splitTime = firstExecution.Split(':');
        if (splitTime.Length != 3)
        {
            throw new ArgumentException("Invalid time format. Please use HH:mm:ss.");
        }
                
        var now = DateTimeHelper.HoraBrasilia();
        var hours = Convert.ToInt32(splitTime[0]) - now.Hour;
        var minutes = Convert.ToInt32(splitTime[1]) - now.Minute;
        var seconds = Convert.ToInt32(splitTime[2]) - now.Second;
        var secondsTillStart = hours * 3600 + minutes * 60 + seconds;

        if (secondsTillStart < 0)
            secondsTillStart = 300;

        return secondsTillStart;        
    }
}
