using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.ValueObjects;

namespace BooKing.Reserve.Application.Dtos;
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
