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
            
            .IsRequired();

        builder.Property(s => s.PerformanceLevel)
            .HasConversion(
                v => v.Name,
                v => SellerPerformanceLevel.From(v))
            .IsRequired()
            .HasMaxLength(32)
            ;

        builder.Metadata
            .FindNavigation(nameof(User.ActiveListings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(u => u.ActiveListings, nav =>
        {
            nav.WithOwner().HasForeignKey("seller_id");
            nav.Property<UserId>("seller_id")
                
                .HasConversion(
                    id => id.Value,
                    value => new UserId(value));
            nav.Property(l => l.Value)
                ;
            nav.HasKey(l => l.Value);

            nav.HasData(DemoSeedData.ActiveListings);
        });

        builder.Property<decimal>("_activeTotalValue")
            
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Ignore(s => s.LimitPolicy);

        builder.HasData(DemoSeedData.Users);
    }
}
