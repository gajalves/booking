using Booking.Reserve.Domain.Entities;
using Booking.Reserve.Domain.ValueObjects;
using BooKing.Generics.Infra.Interfaces;

namespace Booking.Reserve.Domain.Interfaces;
public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration);
}
