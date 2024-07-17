using Booking.Reserve.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Reserve.Infra.Mappings;
public class ReservationMapping : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ApartmentId)
               .IsRequired();

        builder.Property(r => r.UserId)
               .IsRequired();      

        builder.OwnsOne(booking => booking.Duration);                

        builder.Property(r => r.CleaningFee)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(r => r.PriceForPeriod)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(r => r.TotalPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(r => r.Status)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(r => r.CreatedOnUtc);
        builder.Property(r => r.ConfirmedOnUtc);
        builder.Property(r => r.CreatedOnUtc);
        builder.Property(r => r.CompletedOnUtc);
        builder.Property(r => r.CancelledOnUtc);

        builder.ToTable(nameof(Reservation), Schemas.ReservationsSchema);
    }
}
