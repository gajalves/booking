using BooKing.Generics.Domain;

namespace BooKing.Reserve.Application.Erros;
public static class ApplicationErrors
{
    public static class ReserveError
    {        
        public static readonly Error NotAllowedToReserveYourOwnApartment = new Error(
            "ReservationService.Reserve",
            "You'r not allowed to reserve your own Apartment!");
        
        public static readonly Error Overlap = new Error(
            "ReservationService.Reserve",
            "The current reserve is overlapping with an existing one.");

        public static readonly Error NotFound = new Error(
            "ReservationService",
            "Reserve not found.");

        public static readonly Error NotAllowedToConfirmReservation = new Error(
            "ReservationService.Confirm",
            "You'r not allowed to confirm this reservation!");

        public static readonly Error InvalidStatus = new Error(
            "ReservationService.Confirm",
            "Invalid reservation status!");
    }
}
