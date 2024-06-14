using Microsoft.EntityFrameworkCore;

namespace BooKing.Identity.Infra.Context;
public class BooKingIdentityContext : DbContext
{
    protected BooKingIdentityContext()
    {
    }
    public BooKingIdentityContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooKingIdentityContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
