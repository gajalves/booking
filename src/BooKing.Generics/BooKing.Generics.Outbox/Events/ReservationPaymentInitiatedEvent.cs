using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationPaymentInitiatedEvent : Event
{
    public ReservationPaymentInitiatedEvent(Guid reservationId, 
                                            Guid userId, 
                                            decimal totalPrice, 
                                            string userEmail) : base(reservationId)
    {
        ReservationId = reservationId;
        UserId = userId;
        TotalPrice = totalPrice;
        UserEmail = userEmail;
    }

    public Guid ReservationId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public decimal TotalPrice { get; set; }
}
