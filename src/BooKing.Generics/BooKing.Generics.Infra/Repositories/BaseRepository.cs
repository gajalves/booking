using BooKing.Generics.Domain;
using BooKing.Generics.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Infra.Repositories;
public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IUnitOfWork<TContext> _unitOfWork;

    public BaseRepository(DbContext context, IUnitOfWork<TContext> unitOfWork)
    {
        _dbSet = context.Set<TEntity>();
        _unitOfWork = unitOfWork;
    }

    public void Add(TEntity obj)
    {
        _dbSet.Add(obj);
        _unitOfWork.Commit();
        
    }

    public virtual async Task AddAsync(TEntity obj)
    {
        await _dbSet.AddAsync(obj);
        await _unitOfWork.CommitAsync();
    }

    public virtual void Delete(TEntity obj)
    {
        _dbSet.Remove(obj);
        _unitOfWork.Commit();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
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
        _unitOfWork.Commit();
    }
}
