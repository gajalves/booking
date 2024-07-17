using Booking.Reserve.Application.Dtos;
using BooKing.Generics.Domain;

namespace Booking.Reserve.Application.Interfaces;
public interface IReservationService
{
    Task<Result> Reserve(NewReservationDto dto);

    Task CancelReserve(CancelReserveDto dto);
}
