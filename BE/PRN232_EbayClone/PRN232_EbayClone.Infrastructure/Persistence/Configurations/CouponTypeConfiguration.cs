using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CouponTypeConfiguration : IEntityTypeConfiguration<CouponType>
{
    public void Configure(EntityTypeBuilder<CouponType> builder)
    {
        builder.ToTable("coupon_type");

        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Id)
            .HasColumnName("id");

        builder.Property(ct => ct.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ct => ct.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(ct => ct.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(ct => ct.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(ct => ct.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(ct => ct.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(ct => ct.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ct => ct.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.HasQueryFilter(ct => !ct.IsDeleted);

        builder.HasData(GetSeedData());
    }

    private static IEnumerable<object> GetSeedData()
    {
        var seedTimestamp = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        yield return new
        {
            Id = Guid.Parse("9E1D4EA5-5B09-48BE-BE90-E2790F6BA537"),
            Name = "Extra % off",
            Description = "Percentage discount on all eligible items",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("0D0C32FE-349C-4857-B20A-2D3F8DB91ED4"),
            Name = "Extra % off Y or more items",
            Description = "Percentage discount when buying Y or more items",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("CFA2E0F1-B720-4590-A7D4-4CE0844F9671"),
            Name = "Extra $ off $ or more",
            Description = "Fixed amount discount when order value reaches a minimum threshold",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("7EAA19CF-6B36-4A1C-B7B5-A9ABCB7EEFF2"),
            Name = "Buy X get Y at % off",
            Description = "Buy X items and get Y items at a percentage discount",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("ED9D5151-6F8C-4628-A5A9-4C24867E5673"),
            Name = "Buy X get Y free",
            Description = "Buy X items and get Y items for free",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("2C5A6A6A-FE7E-4813-A134-70572B5AB90A"),
            Name = "Extra % off $ or more",
            Description = "Percentage discount when total order value reaches a minimum value",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("773F8D9B-EB8E-4FF4-A21E-4BB2FA5407F4"),
            Name = "Extra $ off X or more items",
            Description = "Fixed amount discount when buying X or more items",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("990C28B3-753E-41B1-A798-965CF46B7DCD"),
            Name = "Extra $ off",
            Description = "Fixed amount discount on all eligible items",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("7A5A0B7A-ED8F-4B91-A7C3-59E5363B76F3"),
            Name = "Extra $ off each item",
            Description = "Fixed amount discount for each item purchased",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("3B980145-62B6-4AE6-9CF8-7838BC7B84E0"),
            Name = "Save $ for every X items",
            Description = "Save a fixed amount for every X items purchased",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };

        yield return new
        {
            Id = Guid.Parse("51F2ED38-06BB-496E-B5CB-7AA3057C21B7"),
            Name = "Save $ for every $ spent",
            Description = "Save a fixed amount for every dollar spent",
            IsActive = true,
            CreatedAt = seedTimestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        };
    }
}
