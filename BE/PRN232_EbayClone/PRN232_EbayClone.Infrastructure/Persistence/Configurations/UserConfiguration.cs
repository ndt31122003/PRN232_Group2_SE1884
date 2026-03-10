using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("user");

        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .ValueGeneratedNever();

        builder
            .Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value));
            
        builder.HasIndex(u => u.Email).IsUnique();

        builder
            .HasMany(u => u.Roles)
            .WithMany();

        builder
            .HasQueryFilter(u => !u.IsDeleted);

        builder
            .Property(u => u.IsPaymentVerified)
            .HasColumnName("is_payment_verified")
            .IsRequired();

        builder.Property(s => s.PerformanceLevel)
            .HasConversion(
                v => v.Name,
                v => SellerPerformanceLevel.From(v))
            .IsRequired()
            .HasMaxLength(32)
            .HasColumnName("performance_level");

        builder.Metadata
            .FindNavigation(nameof(User.ActiveListings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(u => u.ActiveListings, nav =>
        {
            nav.WithOwner().HasForeignKey("seller_id");
            nav.Property<UserId>("seller_id")
                .HasColumnName("seller_id")
                .HasConversion(
                    id => id.Value,
                    value => new UserId(value));
            nav.Property(l => l.Value)
                .HasColumnName("listing_id");
            nav.HasKey(l => l.Value);

            nav.HasData(DemoSeedData.ActiveListings);
        });

        builder.Property<decimal>("_activeTotalValue")
            .HasColumnName("active_total_value")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Ignore(s => s.LimitPolicy);

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(u => u.IsPhoneVerified)
            .HasColumnName("is_phone_verified")
            .IsRequired();

        builder.Property(u => u.BusinessName)
            .HasColumnName("business_name")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(u => u.IsBusinessVerified)
            .HasColumnName("is_business_verified")
            .IsRequired();

        builder.OwnsOne(u => u.BusinessAddress, ba =>
        {
            ba.Property(a => a.Street).HasColumnName("business_street").HasMaxLength(300);
            ba.Property(a => a.City).HasColumnName("business_city").HasMaxLength(100);
            ba.Property(a => a.State).HasColumnName("business_state").HasMaxLength(100);
            ba.Property(a => a.ZipCode).HasColumnName("business_zip_code").HasMaxLength(20);
            ba.Property(a => a.Country).HasColumnName("business_country").HasMaxLength(100);
        });

        builder.HasData(DemoSeedData.Users);
    }
}
