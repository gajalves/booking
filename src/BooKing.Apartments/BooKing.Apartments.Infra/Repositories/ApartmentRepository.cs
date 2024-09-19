using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BooKing.Apartments.Infra.Repositories;
public class ApartmentRepository : BaseRepository<Apartment, BooKingApartmentsContext>, IApartmentRepository
{
    private readonly BooKingApartmentsContext _context;
    private readonly IUnitOfWork<BooKingApartmentsContext> _unitOfWork;

    public static Expression<Func<Apartment, bool>> NotDeleted = a => !a.IsDeleted;

    public ApartmentRepository(BooKingApartmentsContext context, 
                               IUnitOfWork<BooKingApartmentsContext> unitOfWork): base(context, unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<Apartment>().CountAsync();
    }

    public async Task<int> CountByUserIdAsync(Guid userId)
    {
        return await _context.Set<Apartment>()
                             .CountAsync(a => a.OwnerId == userId.ToString());
    }

    public async Task<List<Apartment>> ListPagedAsync(int pageIndex, int pageSize)
    {
        return await _context.Set<Apartment>()
                          .Include(x => x.Amenities)
                          .AsNoTracking()
                          .Where(NotDeleted)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)                          
                          .ToListAsync();
    }

    public override async Task<Apartment> GetByIdAsync(Guid id)
    {
        return await _context.Set<Apartment>()
                             .Include(x => x.Amenities)
                             .Where(a => a.Id == id)
                             .Where(NotDeleted)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<Apartment>> GetApartmentsByGuids(List<Guid> apartmentIds)
    {
        return await _context.Set<Apartment>()
                             .AsNoTracking()
                             .Include(x => x.Amenities)
                             .Where(a => apartmentIds.Contains(a.Id))
                             .Where(NotDeleted)
                             .ToListAsync();
    }

    public async Task<List<Apartment>> GetApartmentsByUserId(Guid userId)
    {
        return await _context.Set<Apartment>()
                             .AsNoTracking()
                             .Include(x => x.Amenities)
                             .Where(a => a.OwnerId == userId.ToString())
                             .Where(NotDeleted)
                             .ToListAsync();
    }

    public override void Delete(Apartment apartment)
    {
        apartment.SetIsDeleted(true);
        apartment.SetIsActive(false);

        base.Update(apartment);
    }
}
