using BooKing.Generics.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooKing.Generics.Outbox.Mappings;
public class OutboxIntegrationEventsMapping : IEntityTypeConfiguration<OutboxIntegrationEvents>
{
    public void Configure(EntityTypeBuilder<OutboxIntegrationEvents> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Queue)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.EventType)
            .HasMaxLength(255);

        builder.Property(o => o.Payload)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.ProcessedAt)
            .IsRequired(false);

        builder.ToTable(nameof(OutboxIntegrationEvents), "Outbox");
    }
}
