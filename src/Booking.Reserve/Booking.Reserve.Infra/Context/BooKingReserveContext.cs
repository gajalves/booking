using Microsoft.EntityFrameworkCore;

namespace BooKing.Reserve.Infra.Context;
public class BooKingReserveContext : DbContext
{
    public BooKingReserveContext()
    {

    }
    public BooKingReserveContext(DbContextOptions<BooKingReserveContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooKingReserveContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
