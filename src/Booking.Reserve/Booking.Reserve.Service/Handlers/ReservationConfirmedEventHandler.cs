using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Domain.Interfaces;

namespace BooKing.Reserve.Service.Handlers;
public class ReservationConfirmedEventHandler : IEventHandler<ReservationConfirmedByUserEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IOutboxEventService _outboxEventService;

    public ReservationConfirmedEventHandler(ILogger<Worker> logger, 
                                            IReservationRepository reservationRepository,
                                            IOutboxEventService outboxEventService)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _outboxEventService = outboxEventService;
    }

    public async Task<bool> Handle(ReservationConfirmedByUserEvent @event)
    {
        try
        {
            _logger.LogInformation($"[ReservationConfirmedEventHandler]: Processing reservation confirmed, ReservationId: {@event.ReservationId}");
            var reservation = await _reservationRepository.GetByIdAsync(@event.ReservationId);
            if (reservation == null)
            {
                await _outboxEventService.SetMessage(@event.EventId, "[ReservationConfirmedEventHandler] Reservation not found");
                return false;
            }

            reservation.ProcessConfirmed();
            _reservationRepository.Update(reservation);

            var paymentInitiatedEvent = new ReservationPaymentInitiatedEvent(@event.ReservationId, reservation.UserId, reservation.TotalPrice, @event.UserEmail);
            await _outboxEventService.AddEvent(QueueMapping.BooKingReservePaymentsInitiated, paymentInitiatedEvent);

            await Task.Delay(3000);
            _logger.LogInformation($"[ReservationConfirmedEventHandler]: Processed reservation confirmed, ReservationId: {@event.ReservationId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"[ReservationConfirmedEventHandler]: ReservationId: {@event.ReservationId} | Ex: {ex.Message}");
            return false;
        }        
    }
}
