using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationCreatedEvent : Event
{
    public ReservationCreatedEvent(Guid reservationId,
                                   Guid userId,
                                   string userEmail,
                                   Guid apartmentId,
                                   string apartmentName,
                                   DateTime start,
                                   DateTime end,
                                   decimal priceForPeriod,
                                   decimal cleaningFee,
                                   decimal totalPrice,
                                   DateTime createdOnUtc) : base(reservationId)
    {
        ReservationId = reservationId;
        UserId = userId;
        UserEmail = userEmail;
        ApartmentId = apartmentId;
        ApartmentName = apartmentName;
        Start = start;
        End = end;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        TotalPrice = totalPrice;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ReservationId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public Guid ApartmentId { get; set; }
    public string ApartmentName { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal PriceForPeriod { get; set; }
    public decimal CleaningFee { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
