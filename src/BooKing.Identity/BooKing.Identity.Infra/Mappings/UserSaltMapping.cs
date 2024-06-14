using BooKing.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooKing.Identity.Infra.Mappings;
public class UserSaltMapping : IEntityTypeConfiguration<UserSalt>
{
    public void Configure(EntityTypeBuilder<UserSalt> builder)
    {
        builder.HasKey(us => us.Id);

        builder.Property(us => us.Salt)
               .IsRequired();

        builder.HasOne(us => us.User)
               .WithOne(u => u.UserSalt)
               .HasForeignKey<UserSalt>(us => us.UserId);

        builder.ToTable(nameof(UserSalt), Schemas.IdentitySchema);
    }
}
