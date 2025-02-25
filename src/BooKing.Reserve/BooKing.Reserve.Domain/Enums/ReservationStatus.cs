using System.ComponentModel;

namespace BooKing.Reserve.Domain.Enums;
public enum ReservationStatus
{
    [Description("Pending Confirmation")]
    Pending = 0,
    [Description("Confirmed")]
    Confirmed = 1,
    [Description("Cancelled")]
    Cancelled = 2,
    [Description("Reserved")]
    Reserved = 3,
    [Description("Completed")]
    Completed = 4,
    [Description("Pending Payment")]
    PendingPayment = 5,
    [Description("Failed Payment")]
    FailedPayment = 6,
    [Description("Payment Completed")]
    PaymentCompleted = 7
}
