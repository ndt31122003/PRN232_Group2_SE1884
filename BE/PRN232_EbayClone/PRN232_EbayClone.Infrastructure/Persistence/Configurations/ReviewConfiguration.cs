using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Reviews.Enums;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("review");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(r => r.ReviewerId)
            .HasColumnName("reviewer_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.ReviewerRole)
            .HasColumnName("reviewer_role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ReviewParticipantRole.Buyer);

        builder.Property(r => r.RecipientId)
            .HasColumnName("recipient_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.RecipientRole)
            .HasColumnName("recipient_role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ReviewParticipantRole.Seller);

        builder.Property(r => r.Rating)
            .HasColumnName("rating")
            .IsRequired();

        // RatingType is computed from Rating, not stored in database
        builder.Ignore(r => r.RatingType);

        builder.Property(r => r.Comment)
            .HasColumnName("comment")
            .HasMaxLength(2000);

        builder.Property(r => r.Reply)
            .HasColumnName("reply")
            .HasMaxLength(2000);

        builder.Property(r => r.RepliedAt)
            .HasColumnName("replied_at");

        builder.Property(r => r.RevisionStatus)
            .HasColumnName("revision_status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(ReviewRevisionStatus.None);

        builder.Property(r => r.RevisionRequestedAt)
            .HasColumnName("revision_requested_at");

        builder.HasOne(r => r.Listing)
            .WithMany()
            .HasForeignKey(r => r.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.ListingId);
        builder.HasIndex(r => r.ReviewerId);
        builder.HasIndex(r => r.RecipientId);
        builder.HasIndex(r => r.RecipientRole);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}

