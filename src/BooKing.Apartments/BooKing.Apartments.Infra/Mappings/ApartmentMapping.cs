using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooKing.Apartments.Infra.Mappings;
public class ApartmentMapping : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(a => a.Description)
               .HasMaxLength(1000);

        builder.OwnsOne(a => a.Address);

        builder.Property(a => a.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(a => a.CleaningFee)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(a => a.LastBookedOnUtc);

        builder.Property(a => a.ImagePath)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(a => a.Amenities)
               .WithMany();               

        builder.Property(a => a.OwnerId)
            .IsRequired()
            .HasMaxLength(255);

        builder.ToTable(nameof(Apartment), Schemas.ApartmentsSchema);        
    }
}
