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
            
            .ValueGeneratedNever();

        builder.Property(r => r.ListingId)
            
            .IsRequired();

        builder.Property(r => r.ReviewerId)
            
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.ReviewerRole)
            
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ReviewParticipantRole.Buyer);

        builder.Property(r => r.RecipientId)
            
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.RecipientRole)
            
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ReviewParticipantRole.Seller);

        builder.Property(r => r.Rating)
            
            .IsRequired();

        // RatingType is computed from Rating, not stored in database
        builder.Ignore(r => r.RatingType);

        builder.Property(r => r.Comment)
            
            .HasMaxLength(2000);

        builder.Property(r => r.Reply)
            
            .HasMaxLength(2000);

        builder.Property(r => r.RepliedAt)
            ;

        builder.Property(r => r.RevisionStatus)
            
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(ReviewRevisionStatus.None);

        builder.Property(r => r.RevisionRequestedAt)
            ;

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

