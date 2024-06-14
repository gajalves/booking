using BooKing.Generics.Domain;
using BooKing.Generics.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Infra.Repositories;
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
{
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public void Add(TEntity obj)
    {
        _dbSet.Add(obj);
    }

    public async Task AddAsync(TEntity obj)
    {
        await _dbSet.AddAsync(obj);
    }

    public void Delete(TEntity obj)
    {
        _dbSet.Remove(obj);
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public void Update(TEntity obj)
    {
        _dbSet.Update(obj);
    }
}
