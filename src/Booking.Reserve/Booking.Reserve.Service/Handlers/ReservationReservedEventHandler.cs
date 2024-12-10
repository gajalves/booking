using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Domain.Interfaces;
using MassTransit;

namespace BooKing.Reserve.Service.Handlers;
public class ReservationReservedEventHandler : IConsumer<ReservationReservedEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IOutboxEventService _outboxEventService;

    public ReservationReservedEventHandler(ILogger<Worker> logger,
                                            IReservationRepository reservationRepository,
                                            IOutboxEventService outboxEventService)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _outboxEventService = outboxEventService;
    }

    public async Task Consume(ConsumeContext<ReservationReservedEvent> context)
    {
        try
        {
            _logger.LogInformation($"[ReservationReservedEventHandler]: Processing set to reserved for ReservationId: {context.Message.ReservationId}");

            var reservation = await _reservationRepository.GetByIdAsync(context.Message.ReservationId);
            if (reservation == null)
            {
                _logger.LogWarning($"[ReservationReservedEventHandler]: Reservation not found for ReservationId: {context.Message.ReservationId}");
            }
            else
            {
                reservation.SetReservedStatus();
                _reservationRepository.Update(reservation);

                _logger.LogInformation($"[ReservationReservedEventHandler]: Reservation {reservation.Id} status updated to Reserved.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[ReservationReservedEventHandler]: Error processing set to reserved for ReservationId: {context.Message.ReservationId}");
        }
    }
}
