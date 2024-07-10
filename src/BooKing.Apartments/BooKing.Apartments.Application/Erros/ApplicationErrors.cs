using BooKing.Apartments.Domain.Entities;
using BooKing.Generics.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

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
    }

    public static class ApplicationError
    {
        public static readonly Error PageIndexAndPageSizeMustBeGreaterThanZero = new Error(
           "ApartmentService",
           "Page index and page size must be greater than zero.");
    }
}
