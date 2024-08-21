using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Apartments.Infra.Repositories;
public class ApartmentRepository : BaseRepository<Apartment, BooKingApartmentsContext>, IApartmentRepository
{
    private readonly BooKingApartmentsContext _context;
    private readonly IUnitOfWork<BooKingApartmentsContext> _unitOfWork;
    
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

    public async Task<List<Apartment>> ListPagedAsync(int pageIndex, int pageSize)
    {
        return await _context.Set<Apartment>()
                          .Include(x => x.Amenities)
                          .AsNoTracking()
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)                          
                          .ToListAsync();
    }

    public override async Task<Apartment> GetByIdAsync(Guid id)
    {
        return await _context.Set<Apartment>()
                             .Include(x => x.Amenities)
                             .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Apartment>> GetApartmentsByGuids(List<Guid> apartmentIds)
    {
        return await _context.Set<Apartment>()
                             .AsNoTracking()
                             .Include(x => x.Amenities)
                             .Where(a => apartmentIds.Contains(a.Id))
                             .ToListAsync();
    }

    public async Task<List<Apartment>> GetApartmentsByUserId(Guid userId)
    {
        return await _context.Set<Apartment>()
                             .AsNoTracking()
                             .Include(x => x.Amenities)
                             .Where(a => a.OwnerId == userId.ToString())
                             .ToListAsync();
    }
}
