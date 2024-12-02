using BooKing.Generics.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace BooKing.Apartments.Application.Erros;
public static class ApplicationErrors
{
    public static class ApartmentError
    {
        public static readonly Error ProvidedApartmentNotFound = new Error(
            "ApartmentService.GetApartmentByIdAsync",
        "Provided apartment id does not exists!");

        public static readonly Error NotAllowedToManageApartment = new Error(
            "ApartmentService",
            "You'r not allowed to manage this Apartment!");
    }

    public static class AmenityError
    {
        public static readonly Error ProvidedAmenityNotFound = new Error(
        "AmenityService.Delete",
        "Provided amenity id does not exists!");

        public static readonly Error InvalidAmenityName = new Error(
        "AmenityService.Create",
        "Amenity name should not be empty!");
    }

    public static class ApplicationError
    {
        public static readonly Error PageIndexAndPageSizeMustBeGreaterThanZero = new Error(
           "ApartmentService",
           "Page index and page size must be greater than zero.");

        public static readonly Error SearchTextCannotBeEmpty = new Error(
           "ApartmentService",
           "Search text cannot be empty.");
    }
}
