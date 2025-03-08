using BooKing.Generics.Infra.Interfaces;
using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace BooKing.Reserve.Domain.Interfaces;
public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration, IDbContextTransaction transaction);
    Task<List<Reservation>> GetReservationsByStatusAndEndDateAsync(ReservationStatus status, DateTime endDate);
    Task<List<Reservation>> GetAllReservationsByUserId(Guid userId);
    Task<Reservation> GetReservation(Guid reservationId);
    Task<int> CountByUserIdAsync(Guid userId);
}
