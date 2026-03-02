using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("dispute");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(d => d.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(d => d.RaisedById)
            .HasColumnName("raised_by_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Reason)
            .HasColumnName("reason")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(d => d.Listing)
            .WithMany()
            .HasForeignKey(d => d.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => d.ListingId);
        builder.HasIndex(d => d.RaisedById);
        builder.HasIndex(d => d.Status);

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}



