using BooKing.Apartments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooKing.Apartments.Infra.Mappings;
public class AmenityMapping : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.ToTable(nameof(Amenity), Schemas.ApartmentsSchema);        
    }    
}
