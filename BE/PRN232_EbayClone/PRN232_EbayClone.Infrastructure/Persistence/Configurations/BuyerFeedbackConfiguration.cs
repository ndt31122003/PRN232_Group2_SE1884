using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Users.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class BuyerFeedbackConfiguration : IEntityTypeConfiguration<BuyerFeedback>
{
    public void Configure(EntityTypeBuilder<BuyerFeedback> builder)
    {
        builder.ToTable("order_buyer_feedback");
        builder.HasKey(feedback => feedback.Id);

        builder.Property(feedback => feedback.Id)
            .HasColumnName("id");

        builder.Property(feedback => feedback.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(feedback => feedback.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();

        builder.Property(feedback => feedback.BuyerId)
            .HasColumnName("buyer_id")
            .IsRequired();

        builder.Property(feedback => feedback.Comment)
            .HasColumnName("comment")
            .HasMaxLength(BuyerFeedback.MaxCommentLength)
            .IsRequired();

        builder.Property(feedback => feedback.UsesStoredComment)
            .HasColumnName("uses_stored_comment")
            .IsRequired();

        builder.Property(feedback => feedback.StoredCommentKey)
            .HasColumnName("stored_comment_key")
            .HasMaxLength(100);

        builder.Property(feedback => feedback.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(feedback => feedback.FollowUpComment)
            .HasColumnName("follow_up_comment")
            .HasMaxLength(BuyerFeedback.MaxCommentLength);

        builder.Property(feedback => feedback.FollowUpCommentedAt)
            .HasColumnName("follow_up_commented_at");

        builder.HasIndex(feedback => feedback.OrderId)
            .IsUnique()
            .HasDatabaseName("ux_buyer_feedback_order");

        builder.HasOne<Order>()
            .WithOne(order => order.SellerFeedback)
            .HasForeignKey<BuyerFeedback>(feedback => feedback.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(feedback => feedback.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
