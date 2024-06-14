using BooKing.Generics.Domain;

namespace BooKing.Generics.Infra.Interfaces;
public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity obj);
    void Add(TEntity obj);
    Task<IEnumerable<TEntity>> ListAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    void Delete(TEntity obj);
    void Update(TEntity obj);

}
