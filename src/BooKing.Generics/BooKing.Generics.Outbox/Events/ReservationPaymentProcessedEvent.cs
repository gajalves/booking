using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationPaymentProcessedEvent : Event
{
    public ReservationPaymentProcessedEvent(Guid reservationId, 
                                            string userEmail, 
                                            decimal totalPrice, 
                                            bool isApproved) : base(reservationId)
    {
        ReservationId = reservationId;
        UserEmail = userEmail;        
        TotalPrice = totalPrice;
        IsApproved = isApproved;
    }

    public Guid ReservationId { get; set; }
    public string UserEmail { get; set; }    
    public decimal TotalPrice { get; set; }
    public bool IsApproved { get; set; }
}
