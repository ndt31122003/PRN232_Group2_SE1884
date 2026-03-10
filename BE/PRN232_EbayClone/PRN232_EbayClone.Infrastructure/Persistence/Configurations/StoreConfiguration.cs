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
            
            .IsRequired();

        builder.Property(s => s.Name)
            
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Slug)
            
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(s => s.Slug).IsUnique();

        builder.Property(s => s.Description)
            ;

        builder.Property(s => s.LogoUrl)
            
            .HasMaxLength(500);

        builder.Property(s => s.BannerUrl)
            
            .HasMaxLength(500);

        builder.Property(s => s.StoreType)
            
            .IsRequired();

        builder.Property(s => s.IsActive)
            
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

