using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Infra;
public interface IUnitOfWork<TContext> where TContext : DbContext
{
    Task Commit();
}
