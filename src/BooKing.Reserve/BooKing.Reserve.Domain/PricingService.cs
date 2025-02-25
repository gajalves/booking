﻿using BooKing.Reserve.Domain.ValueObjects;

namespace BooKing.Reserve.Domain;
public class PricingService
{
    public PricingDetails CalculatePrice(decimal price, decimal cleaningFee, DateRange period)
    {
        var priceForPeriod = price * period.lengthInDays;

        var totalPrice = priceForPeriod + cleaningFee;

        return new PricingDetails(priceForPeriod, cleaningFee, totalPrice);
    }
}
