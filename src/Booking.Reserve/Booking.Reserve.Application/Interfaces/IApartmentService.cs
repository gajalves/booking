using Booking.Reserve.Application.Dtos;
using BooKing.Generics.Domain;

namespace Booking.Reserve.Application.Interfaces;
public interface IApartmentService
{
    Task<Result<GetApartmentDto>> GetApartment(Guid apartmentId);
}
