using BooKing.Reserve.Application.Interfaces;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Service;

namespace BooKing.Reserve.Service.Handlers;
public class ReservationPaymentInitiatedEventHandler : IEventHandler<ReservationPaymentInitiatedEvent>
{
    private readonly ILogger<Worker> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IOutboxEventService _outboxEventService;
    private readonly IPaymentService _paymentService;

    public ReservationPaymentInitiatedEventHandler(ILogger<Worker> logger,
                                                   IReservationRepository reservationRepository,
                                                   IOutboxEventService outboxEventService,
                                                   IPaymentService paymentService)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _outboxEventService = outboxEventService;
        _paymentService = paymentService;
    }

    public async Task<bool> Handle(ReservationPaymentInitiatedEvent @event)
    {
        try
        {
            _logger.LogInformation($"[ReservationPaymentInitiatedEventHandler]: Initiating payment for ReservationId: {@event.ReservationId}");
            
            var reservation = await _reservationRepository.GetByIdAsync(@event.ReservationId);
            if (reservation == null)
            {
                await _outboxEventService.SetMessage(@event.EventId, "[ReservationPaymentInitiatedEventHandler] Reservation not found");
                return false;
            }

            var paymentResult = await _paymentService.ProcessPaymentAsync(@event.ReservationId, @event.TotalPrice, true);

            var ev = new ReservationPaymentProcessedEvent(reservation.Id, @event.UserEmail, reservation.TotalPrice, paymentResult.IsSuccess);            
            await _outboxEventService.AddEvent(QueueMapping.BooKingEmailServicePaymentProcessed, ev);

            if (!paymentResult.IsSuccess)
            {
                reservation.SetFailedPaymentStatus();
                _reservationRepository.Update(reservation);
                                
                _logger.LogWarning($"[ReservationPaymentInitiatedEventHandler]: Payment failed for ReservationId: {@event.ReservationId}");                
                return false;
            }
                        
            reservation.MarkPaymentCompleted();
            _reservationRepository.Update(reservation);

            var reservedEvent = new ReservationReservedEvent(reservation.Id);
            await _outboxEventService.AddEvent(QueueMapping.BooKingReserveReservationReserved, reservedEvent);

            _logger.LogInformation($"[ReservationPaymentInitiatedEventHandler]: Payment successful for ReservationId: {@event.ReservationId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[ReservationPaymentInitiatedEventHandler]: Error processing payment for ReservationId: {@event.ReservationId}");
            return false;
        }
    }
}
