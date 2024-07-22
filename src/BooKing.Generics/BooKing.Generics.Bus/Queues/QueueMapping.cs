namespace BooKing.Generics.Bus.Queues;
public static class QueueMapping
{
    public const string NoQueueNeeded = "no-queue-needed.just-event";

    public const string BooKingEmailServiceNewUser = "booking-email-service.new-user";
    public const string BooKingEmailServiceReservationCreated = "booking-email-service.reservation-created";
    public const string BooKingEmailServicePaymentProcessed = "booking-email-service.payment-processed";
    
    public const string BooKingReserveReservationConfirmed = "booking-reserve-service.reservation-confirmed";
    public const string BooKingReserveReservationCancelled = "booking-email-service.reservation-cancelled";
    public const string BooKingReservePaymentsInitiated = "booking-reserve-service.payments-initiated";
    public const string BooKingReserveReservationReserved = "booking-reserve-service.reservation-reserved";
}
