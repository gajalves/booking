using BooKing.Apartments.Domain.Entities;
using BooKing.Generics.Infra.Interfaces;

namespace BooKing.Apartments.Domain.Interfaces;
public interface IApartmentRepository : IBaseRepository<Apartment>
{
    Task<List<Apartment>> ListPagedAsync(int pageIndex, int pageSize);
    Task<int> CountAsync();
}
