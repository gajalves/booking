using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Domain.Interfaces;
using MassTransit;

namespace BooKing.Reserve.Service.Handlers;
public class ReservationPaymentInitiatedEventHandler : IConsumer<ReservationPaymentInitiatedEvent>
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

    public async Task Consume(ConsumeContext<ReservationPaymentInitiatedEvent> context)
    {
        try
        {
            _logger.LogInformation($"[ReservationPaymentInitiatedEventHandler]: Initiating payment for ReservationId: {context.Message.ReservationId}");

            var reservation = await _reservationRepository.GetByIdAsync(context.Message.ReservationId);
            if (reservation == null)
            {
                await _outboxEventService.SetMessage(context.Message.EventId, "[ReservationPaymentInitiatedEventHandler] Reservation not found");
            }
            else
            {
                var paymentResult = await _paymentService.ProcessPaymentAsync(context.Message.ReservationId, context.Message.TotalPrice, true);

                var ev = new ReservationPaymentProcessedEvent(reservation.Id, context.Message.UserEmail, reservation.TotalPrice, paymentResult.IsSuccess);
                await _outboxEventService.AddEvent(ev);

                if (!paymentResult.IsSuccess)
                {
                    reservation.SetFailedPaymentStatus();
                    _reservationRepository.Update(reservation);

                    _logger.LogWarning($"[ReservationPaymentInitiatedEventHandler]: Payment failed for ReservationId: {context.Message.ReservationId}");
                }

                reservation.MarkPaymentCompleted();
                _reservationRepository.Update(reservation);

                var reservedEvent = new ReservationReservedEvent(reservation.Id);
                await _outboxEventService.AddEvent(reservedEvent);

                _logger.LogInformation($"[ReservationPaymentInitiatedEventHandler]: Payment successful for ReservationId: {context.Message.ReservationId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[ReservationPaymentInitiatedEventHandler]: Error processing payment for ReservationId: {context.Message.ReservationId}");
        }
    }
}
