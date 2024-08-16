using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.ValueObjects;
using BooKing.Generics.Infra.Interfaces;
using BooKing.Reserve.Domain.Enums;

namespace BooKing.Reserve.Domain.Interfaces;
public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration);
    Task<List<Reservation>> GetReservationsByStatusAndEndDateAsync(ReservationStatus status, DateTime endDate);
    Task<List<Reservation>> GetAllReservationsByUserId(Guid userId);
    Task<Reservation> GetReservation(Guid reservationId);
}
