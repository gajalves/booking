namespace BooKing.Generics.Outbox.Events;
public class PaymentInitiatedEvent : Event
{
    public PaymentInitiatedEvent(Guid reservationId, Guid userId, decimal totalPrice)
    {
        ReservationId = reservationId;
        UserId = userId;
        TotalPrice = totalPrice;
    }

    public Guid ReservationId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
}
