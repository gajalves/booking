namespace BooKing.Generics.Outbox.Events;
public class ReservationCompletedEvent : Event
{
    public ReservationCompletedEvent(Guid reservationId)
    {
        ReservationId = reservationId;
    }

    public Guid ReservationId { get; }
}
