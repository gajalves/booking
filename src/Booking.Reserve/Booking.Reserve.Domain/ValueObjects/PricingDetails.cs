namespace Booking.Reserve.Domain.ValueObjects;
public record PricingDetails(
    decimal PriceForPeriod,
    decimal CleaningFee,    
    decimal TotalPrice);