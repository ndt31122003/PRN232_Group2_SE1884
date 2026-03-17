using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Users.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class SellerReviewReplyConfiguration : IEntityTypeConfiguration<SellerReviewReply>
{
    public void Configure(EntityTypeBuilder<SellerReviewReply> builder)
    {
        builder.ToTable("seller_review_reply");
        builder.HasKey(reply => reply.Id);

        builder.Property(reply => reply.Id)
            .HasColumnName("id");

        builder.Property(reply => reply.ReviewId)
            .HasColumnName("review_id")
            .IsRequired();

        builder.Property(reply => reply.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();

        builder.Property(reply => reply.ReplyText)
            .HasColumnName("reply_text")
            .HasMaxLength(SellerReviewReply.MaxReplyLength)
            .IsRequired();

        builder.Property(reply => reply.RepliedAt)
            .HasColumnName("replied_at")
            .IsRequired();

        builder.Property(reply => reply.EditedAt)
            .HasColumnName("edited_at");

        builder.Property(reply => reply.IsEdited)
            .HasColumnName("is_edited")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(reply => reply.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(reply => reply.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(reply => reply.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(reply => reply.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(reply => reply.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(reply => reply.ReviewId)
            .HasDatabaseName("ix_seller_review_reply_review_id");

        builder.HasIndex(reply => reply.SellerId)
            .HasDatabaseName("ix_seller_review_reply_seller_id");

        builder.HasIndex(reply => reply.ReviewId)
            .IsUnique()
            .HasDatabaseName("ux_seller_review_reply_review")
            .HasFilter("is_deleted = false");

        builder.HasOne<BuyerFeedback>()
            .WithMany()
            .HasForeignKey(reply => reply.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(reply => reply.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
