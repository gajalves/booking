using Booking.Reserve.Domain.Enums;
using Booking.Reserve.Domain.Interfaces;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;

namespace Booking.Reserve.Service.Handlers;
public class ReservationConfirmedEventHandler : IEventHandler<ReservationConfirmedByUserEvent>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IOutboxEventService _outboxEventService;

    public ReservationConfirmedEventHandler(IReservationRepository reservationRepository, 
                                            IOutboxEventService outboxEventService)
    {
        _reservationRepository = reservationRepository;
        _outboxEventService = outboxEventService;
    }

    public async Task<bool> Handle(ReservationConfirmedByUserEvent @event)
    {
        var reservation = await _reservationRepository.GetByIdAsync(@event.ReservationId);
        reservation.ProcessConfirmed();

        _reservationRepository.Update(reservation);
        
        var paymentInitiatedEvent = new PaymentInitiatedEvent(@event.ReservationId, reservation.UserId, reservation.TotalPrice);
        await _outboxEventService.AddEvent(QueueMapping.BookingReservePaymentsInitiated, paymentInitiatedEvent);
        return true;
    }
}
