﻿using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.ValueObjects;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared;

namespace BooKing.Reserve.Domain.Entities;
public class Reservation : Entity
{
    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }    
    public decimal PriceForPeriod { get; private set; }
    public decimal CleaningFee { get; private set; }
    public decimal TotalPrice { get; private set; }
    public ReservationStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }


    public Reservation()
    {        
    }

    private Reservation(Guid apartmentId,
                       Guid userId,
                       DateRange duration,
                       decimal priceForPeriod,
                       decimal cleaningFee,
                       decimal totalPrice)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        TotalPrice = totalPrice;
        Status = ReservationStatus.Pending;
        CreatedOnUtc = DateTimeHelper.HoraBrasilia();
    }

    public static Reservation Reserve(Guid apartmentId, 
                                      Guid userId,
                                      decimal price, 
                                      decimal cleaningFee,
                                      DateRange duration, 
                                      PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(price, cleaningFee, duration);

        var reservation = new Reservation(
            apartmentId,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.TotalPrice);

        return reservation;
    }    

    public void Confirm()
    {
        Status = ReservationStatus.Confirmed;
        ConfirmedOnUtc = DateTimeHelper.HoraBrasilia();
    }

    public void Cancel()
    {
        Status = ReservationStatus.Cancelled;
        CancelledOnUtc = DateTimeHelper.HoraBrasilia();
    }

    public void ProcessConfirmed()
    {
        Status = ReservationStatus.PendingPayment;        
    }

    public void SetFailedPaymentStatus()
    {
        Status = ReservationStatus.FailedPayment;
    }

    public void SetReservedStatus()
    {
        Status = ReservationStatus.Reserved;
    }

    public void SetCompletedStatus()
    {
        Status = ReservationStatus.Completed;
        CompletedOnUtc = DateTimeHelper.HoraBrasilia();
    }

    public void MarkPaymentCompleted()
    {
        Status = ReservationStatus.PaymentCompleted;
    }        
}
