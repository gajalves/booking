namespace BooKing.Generics.Outbox.Events;
public class ReservationConfirmedByUserEvent : Event
{
    public ReservationConfirmedByUserEvent(Guid reservationId)
    {
        ReservationId = reservationId;
    }

    public Guid ReservationId { get; set; }
}
