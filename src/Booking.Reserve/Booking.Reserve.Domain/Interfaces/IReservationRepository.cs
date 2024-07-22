using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.ValueObjects;
using BooKing.Generics.Infra.Interfaces;

namespace BooKing.Reserve.Domain.Interfaces;
public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration);
}
