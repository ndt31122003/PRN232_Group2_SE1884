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
            ;

        builder.Property(feedback => feedback.OrderId)
            
            .IsRequired();

        builder.Property(feedback => feedback.SellerId)
            
            .IsRequired();

        builder.Property(feedback => feedback.BuyerId)
            
            .IsRequired();

        builder.Property(feedback => feedback.Comment)
            
            .HasMaxLength(BuyerFeedback.MaxCommentLength)
            .IsRequired();

        builder.Property(feedback => feedback.UsesStoredComment)
            
            .IsRequired();

        builder.Property(feedback => feedback.StoredCommentKey)
            
            .HasMaxLength(100);

        builder.Property(feedback => feedback.CreatedAt)
            
            .IsRequired();

        builder.Property(feedback => feedback.FollowUpComment)
            
            .HasMaxLength(BuyerFeedback.MaxCommentLength);

        builder.Property(feedback => feedback.FollowUpCommentedAt)
            ;

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
