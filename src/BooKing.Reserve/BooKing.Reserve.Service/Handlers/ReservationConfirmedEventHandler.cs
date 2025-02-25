using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Reserve.Domain.Interfaces;
using MassTransit;

namespace BooKing.Reserve.Service.Handlers;
public class ReservationConfirmedEventHandler : IConsumer<ReservationConfirmedByUserEvent>
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

    public async Task Consume(ConsumeContext<ReservationConfirmedByUserEvent> context)
    {
        try
        {
            _logger.LogInformation($"[ReservationConfirmedEventHandler]: Processing reservation confirmed, ReservationId: {context.Message.ReservationId}");
            var reservation = await _reservationRepository.GetByIdAsync(context.Message.ReservationId);
            if (reservation == null)
            {
                await _outboxEventService.SetMessage(context.Message.EventId, "[ReservationConfirmedEventHandler] Reservation not found");

            }
            else
            {
                reservation.ProcessConfirmed();
                _reservationRepository.Update(reservation);

                var paymentInitiatedEvent = new ReservationPaymentInitiatedEvent(context.Message.ReservationId, reservation.UserId, reservation.TotalPrice, context.Message.UserEmail);
                await _outboxEventService.AddEvent(paymentInitiatedEvent);

                await Task.Delay(3000);
                _logger.LogInformation($"[ReservationConfirmedEventHandler]: Processed reservation confirmed, ReservationId: {context.Message.ReservationId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"[ReservationConfirmedEventHandler]: ReservationId: {context.Message.ReservationId} | Ex: {ex.Message}");
        }
    }
}
