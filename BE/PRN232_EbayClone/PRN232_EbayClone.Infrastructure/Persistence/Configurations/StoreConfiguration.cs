using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Stores.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("store");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => new StoreId(value))
            .ValueGeneratedNever();

        builder.Property(s => s.UserId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Slug)
            .HasColumnName("slug")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(s => s.Slug).IsUnique();

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.LogoUrl)
            .HasColumnName("logo_url")
            .HasMaxLength(500);

        builder.Property(s => s.BannerUrl)
            .HasColumnName("banner_url")
            .HasMaxLength(500);

        builder.Property(s => s.StoreType)
            .HasColumnName("store_type")
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.HasMany(s => s.Subscriptions)
            .WithOne()
            .HasForeignKey("StoreId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);

        builder.HasIndex(s => s.UserId)
            .HasDatabaseName("idx_store_user_id");

        builder.HasIndex(s => s.Slug)
            .HasDatabaseName("idx_store_slug");
    }
}

