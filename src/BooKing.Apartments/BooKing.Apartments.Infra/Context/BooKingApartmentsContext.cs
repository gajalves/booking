using Microsoft.EntityFrameworkCore;

namespace BooKing.Apartments.Infra.Context;
public class BooKingApartmentsContext : DbContext
{
    public BooKingApartmentsContext()
    {
        
    }
    public BooKingApartmentsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooKingApartmentsContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
