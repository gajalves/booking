using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationReservedEvent : Event
{
    public Guid ReservationId { get; }

    public ReservationReservedEvent(Guid reservationId) : base(reservationId)
    {
        ReservationId = reservationId;
    }
}
