using BooKing.Generics.Domain;

namespace Booking.Reserve.Application.Erros;
public static class ApplicationErrors
{
    public static class ReserveError
    {        
        public static readonly Error NotAllowedToReserveYourOwnApartment = new Error(
            "ReservationService.Reserve",
            "You'r not allowed to reserve your own Apartment!");
        
        public static readonly Error Overlap = new Error(
            "ReservationService.Reserve",
            "The current reserve is overlapping with an existing one");
    }
}
