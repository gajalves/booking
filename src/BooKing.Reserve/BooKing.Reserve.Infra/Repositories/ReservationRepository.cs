using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Domain.ValueObjects;
using BooKing.Reserve.Infra.Context;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace BooKing.Reserve.Infra.Repositories;
public class ReservationRepository : BaseRepository<Reservation, BooKingReserveContext>, IReservationRepository
{
    private readonly BooKingReserveContext _context;
    private readonly IUnitOfWork<BooKingReserveContext> _unitOfWork;

    private static readonly ReservationStatus[] ActiveBooKingStatuses =
    {
        ReservationStatus.Pending,
        ReservationStatus.Reserved,
        ReservationStatus.Confirmed,
        ReservationStatus.PendingPayment,
        ReservationStatus.PaymentCompleted
    };

    public ReservationRepository(BooKingReserveContext context,
                                 IUnitOfWork<BooKingReserveContext> unitOfWork) : base(context, unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        return await _context.Database.BeginTransactionAsync(isolationLevel);
    }

    public async Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration, IDbContextTransaction transaction)
    {
        try
        {
            var sql = @"
            SELECT 1
            FROM Reservations.Reservation WITH (UPDLOCK, HOLDLOCK)
            WHERE ApartmentId = @apartmetnId
            AND Status IN @statuses
            AND (
                (CAST(@startDate AS DATE) BETWEEN CAST(Duration_Start AS DATE) AND CAST(Duration_End AS DATE))
                OR (CAST(@endDate AS DATE) BETWEEN CAST(Duration_Start AS DATE) AND CAST(Duration_End AS DATE))
                OR (CAST(Duration_Start AS DATE) BETWEEN CAST(@startDate AS DATE) AND CAST(@endDate AS DATE))
                OR (CAST(Duration_End AS DATE) BETWEEN CAST(@startDate AS DATE) AND CAST(@endDate AS DATE))
            )";

            var parameters = new
            {
                apartmetnId,
                startDate = duration.Start,
                endDate = duration.End,
                statuses = ActiveBooKingStatuses
            };

            var connection = _context.Database.GetDbConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, parameters, transaction: transaction.GetDbTransaction());
            return result != 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<Reservation>> GetReservationsByStatusAndEndDateAsync(ReservationStatus status, DateTime endDate)
    {
        return await _context.Set<Reservation>()
                             .Where(r => r.Status == status &&
                                         r.Duration.End.Date <= endDate.Date)
                             .ToListAsync();
    }

    public async Task<List<Reservation>> GetAllReservationsByUserId(Guid userId)
    {
        return await _context.Set<Reservation>()
                             .Where(r => r.UserId == userId)
                             .AsNoTracking()
                             .ToListAsync();
    }

    public async Task<Reservation> GetReservation(Guid reservationId)
    {
        return await _context.Set<Reservation>()
                        .Where(r => r.Id == reservationId)
                        .FirstOrDefaultAsync();
    }

    public async Task<int> CountByUserIdAsync(Guid userId)
    {
        return await _context.Set<Reservation>()
                             .CountAsync(a => a.UserId == userId);
    }
}
