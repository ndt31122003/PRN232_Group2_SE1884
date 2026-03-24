using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("support_tickets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.SellerId)
            .HasColumnName("seller_id")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Subject)
            .HasColumnName("subject")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasColumnName("message")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Ignore audit properties that don't exist in the database
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedBy);

        // Indexes
        builder.HasIndex(x => x.SellerId)
            .HasDatabaseName("idx_support_tickets_seller_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("idx_support_tickets_status");

        builder.HasIndex(x => x.Category)
            .HasDatabaseName("idx_support_tickets_category");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("idx_support_tickets_created_at");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("idx_support_tickets_is_deleted");

        builder.HasIndex(x => new { x.SellerId, x.Status })
            .HasDatabaseName("idx_support_tickets_seller_status")
            .HasFilter("is_deleted = false");

        // Soft delete filter
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}