using BooKing.Reserve.Domain.ValueObjects;

namespace BooKing.Reserve.Application.Dtos;
public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ApartmentId { get; set; }
    public GetApartmentDto Apartment { get; set; }
    public DateRange Duration { get; set; }
    public decimal PriceForPeriod { get; set; }
    public decimal CleaningFee { get; set; }
    public decimal TotalPrice { get; set; }
    public string StatusDescription { get; set; }
    public int StatusValue { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ConfirmedOnUtc { get; set; }
    public DateTime? RejectedOnUtc { get; set; }
    public DateTime? CompletedOnUtc { get; set; }
    public DateTime? CancelledOnUtc { get; set; }
}
