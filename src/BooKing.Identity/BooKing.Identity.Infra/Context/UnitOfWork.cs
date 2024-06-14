using BooKing.Generics.Domain;

namespace BooKing.Identity.Infra.Context;
public class UnitOfWork : IUnitOfWork
{
    private readonly BooKingIdentityContext _context;

    public UnitOfWork(BooKingIdentityContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}
