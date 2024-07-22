using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Service;

namespace Booking.Reserve.Service.Handlers;
public class ReservationReservedEventHandler : IEventHandler<ReservationReservedEvent>
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

    public async Task<bool> Handle(ReservationReservedEvent @event)
    {
        try
        {
            _logger.LogInformation($"[ReservationReservedEventHandler]: Processing set to reserved for ReservationId: {@event.ReservationId}");

            var reservation = await _reservationRepository.GetByIdAsync(@event.ReservationId);
            if (reservation == null)
            {
                _logger.LogWarning($"[ReservationReservedEventHandler]: Reservation not found for ReservationId: {@event.ReservationId}");
                return false;
            }

            reservation.SetReservedStatus();
            _reservationRepository.Update(reservation);

            _logger.LogInformation($"[ReservationReservedEventHandler]: Reservation {reservation.Id} status updated to Reserved.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[ReservationReservedEventHandler]: Error processing set to reserved for ReservationId: {@event.ReservationId}");
            return false;
        }
    }
}
