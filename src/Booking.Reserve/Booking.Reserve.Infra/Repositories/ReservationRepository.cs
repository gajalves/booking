using Booking.Reserve.Domain.Entities;
using Booking.Reserve.Domain.Enums;
using Booking.Reserve.Domain.Interfaces;
using Booking.Reserve.Domain.ValueObjects;
using Booking.Reserve.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.Reserve.Infra.Repositories;
public class ReservationRepository : BaseRepository<Reservation, BooKingReserveContext>, IReservationRepository
{
    private readonly BooKingReserveContext _context;
    private readonly IUnitOfWork<BooKingReserveContext> _unitOfWork;

    private static readonly ReservationStatus[] ActiveBookingStatuses =
    {
        ReservationStatus.Reserved,
        ReservationStatus.Confirmed,
        ReservationStatus.Completed,
        ReservationStatus.PendingPayment
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
                               ActiveBookingStatuses.Contains(r.Status));
    }
}
