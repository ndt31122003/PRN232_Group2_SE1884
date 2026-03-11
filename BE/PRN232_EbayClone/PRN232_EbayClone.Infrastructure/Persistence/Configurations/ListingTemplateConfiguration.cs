using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class ListingTemplateConfiguration : IEntityTypeConfiguration<ListingTemplate>
{
    public void Configure(EntityTypeBuilder<ListingTemplate> builder)
    {
        builder.ToTable("listing_template");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.FormatLabel)
            .HasMaxLength(50);

        builder.Property(t => t.ThumbnailUrl)
            .HasMaxLength(500);

        builder.Property(t => t.PayloadJson)
            .IsRequired()
            .HasColumnName("payload_json")
            .HasColumnType("jsonb");

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(t => t.IsDeleted)
            .HasColumnName("is_deleted");

        builder.HasIndex(t => t.Name)
            .HasDatabaseName("ix_listing_template_name");

        builder.HasData(DemoSeedData.ListingTemplates);

        builder
            .HasQueryFilter(t => !t.IsDeleted);
    }
}
