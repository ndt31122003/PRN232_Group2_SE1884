using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder
            .ToTable("otp");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
             .Property(u => u.Email)
             .HasColumnName("email")
             .HasConversion(
                 e => e.Value,
                 value => new Email(value));

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder
            .HasIndex(x => new { x.Email, x.Code })
            .IsUnique();
    }
}
