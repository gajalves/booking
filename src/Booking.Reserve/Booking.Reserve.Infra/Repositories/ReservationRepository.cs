using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Domain.ValueObjects;
using BooKing.Reserve.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> IsOverlappingAsync(Guid apartmetnId, DateRange duration)
    {
        return await _context.Set<Reservation>()
                .AnyAsync(r => r.ApartmentId == apartmetnId &&
                               r.Duration.Start <= duration.End &&
                               r.Duration.End >= duration.Start &&
                               ActiveBooKingStatuses.Contains(r.Status));
    }
}
