using BooKing.Apartments.Domain.Entities;
using BooKing.Generics.Infra.Interfaces;

namespace BooKing.Apartments.Domain.Interfaces;
public interface IAmenityRepository : IBaseRepository<Amenity>
{
    Task<List<Amenity>> GetAllAsync();
}
