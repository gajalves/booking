using Booking.Reserve.Domain.Enums;
using Booking.Reserve.Domain.ValueObjects;

namespace Booking.Reserve.Application.Dtos;
public class ReservationCreatedDto
{
    public Guid ApartmentId { get; set; }    
    public DateRange Duration { get; set; }
    public decimal PriceForPeriod { get; set; }
    public decimal CleaningFee { get; set; }
    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedOnUtc { get; set; }    
}
