using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationCompletedEvent : Event
{
    public ReservationCompletedEvent(Guid reservationId) : base(reservationId)
    {
        ReservationId = reservationId;
    }

    public Guid ReservationId { get; }
}
