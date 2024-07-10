using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Apartments.Infra.Repositories;
public class AmenityRepository : BaseRepository<Amenity, BooKingApartmentsContext>, IAmenityRepository
{
    private readonly BooKingApartmentsContext _context;
    private readonly IUnitOfWork<BooKingApartmentsContext> _unitOfWork;

    public AmenityRepository(BooKingApartmentsContext context, 
                             IUnitOfWork<BooKingApartmentsContext> unitOfWork) : base(context, unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public Task<List<Amenity>> GetAllAsync()
    {
        return _context.Set<Amenity>().AsNoTracking().ToListAsync();
    }
}
