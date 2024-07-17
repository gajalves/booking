using Microsoft.EntityFrameworkCore;

namespace Booking.Reserve.Infra.Context;
public class BooKingReserveContext : DbContext
{
    public BooKingReserveContext()
    {

    }
    public BooKingReserveContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooKingReserveContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
