using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Outbox.Context;
public class OutboxContext : DbContext
{
    protected OutboxContext()
    {
    }

    public OutboxContext(DbContextOptions<OutboxContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OutboxContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
