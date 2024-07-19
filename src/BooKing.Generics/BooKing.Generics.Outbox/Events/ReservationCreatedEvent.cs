namespace BooKing.Generics.Outbox.Events;
public class ReservationCreatedEvent : Event
{
    public ReservationCreatedEvent(Guid reservationId,                                   
                                   string userEmail, 
                                   string apartmentName, 
                                   DateTime start, 
                                   DateTime end, 
                                   decimal totalPrice)
    {
        ReservationId = reservationId;        
        UserEmail = userEmail;
        ApartmentName = apartmentName;
        Start = start;
        End = end;
        TotalPrice = totalPrice;
    }

    public Guid ReservationId { get; set; }    
    public string UserEmail { get; set; }
    public string ApartmentName { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal TotalPrice { get; set; }
}
