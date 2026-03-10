using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .ToTable("refresh_token");

        builder
            .HasKey(rt => rt.Id);

        builder
            .Property(u => u.UserId)
            
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.
            Property(rt => rt.Token)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(rt => rt.ExpiresOnUtc)
            .IsRequired();

        builder
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId);

        builder
            .HasQueryFilter(rt => !rt.IsDeleted);
    }
}
