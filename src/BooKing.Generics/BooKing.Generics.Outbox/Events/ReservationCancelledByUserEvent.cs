using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationCancelledByUserEvent : Event
{
    public ReservationCancelledByUserEvent(Guid reservationId, 
                                           Guid userId, 
                                           string userEmail, 
                                           DateTime start, 
                                           DateTime end, 
                                           decimal totalPrice) : base(reservationId)
    {
        ReservationId = reservationId;
        UserId = userId;
        UserEmail = userEmail;
        Start = start;
        End = end;
        TotalPrice = totalPrice;
    }

    public Guid ReservationId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal TotalPrice { get; set; }
}
