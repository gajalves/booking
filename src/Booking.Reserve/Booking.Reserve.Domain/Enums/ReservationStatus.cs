namespace BooKing.Reserve.Domain.Enums;
public enum ReservationStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Reserved = 3,
    Completed = 4,
    PendingPayment = 5,
    FailedPayment = 6,
    PaymentCompleted = 7
}
