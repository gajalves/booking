using BooKing.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooKing.Identity.Infra.Mappings;
public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Password)
               .IsRequired();

        builder.HasOne(u => u.UserSalt)
                   .WithOne()
                   .HasForeignKey<UserSalt>(us => us.UserId);
        builder.ToTable(nameof(User), Schemas.IdentitySchema);        
    }
}
