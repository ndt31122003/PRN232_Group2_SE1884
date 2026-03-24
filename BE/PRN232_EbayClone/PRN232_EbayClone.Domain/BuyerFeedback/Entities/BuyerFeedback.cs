using PRN232_EbayClone.Domain.BuyerFeedback.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.BuyerFeedback.Entities;

public class BuyerFeedbackEntity : AggregateRoot<Guid>
{
    public string SellerId { get; private set; } = string.Empty;
    public string BuyerId { get; private set; } = string.Empty;
    public Guid? OrderId { get; private set; }
    public Guid? ListingId { get; private set; }
    public FeedbackType FeedbackType { get; private set; }
    public FeedbackReason? Reason { get; private set; }
    public string? Comment { get; private set; }

    private BuyerFeedbackEntity() : base(Guid.Empty) { } // EF Core

    private BuyerFeedbackEntity(
        Guid id,
        string sellerId,
        string buyerId,
        Guid? orderId,
        Guid? listingId,
        FeedbackType feedbackType,
        FeedbackReason? reason,
        string? comment) : base(id)
    {
        SellerId = sellerId;
        BuyerId = buyerId;
        OrderId = orderId;
        ListingId = listingId;
        FeedbackType = feedbackType;
        Reason = reason;
        Comment = comment;
    }

    public static Result<BuyerFeedbackEntity> Create(
        string sellerId,
        string buyerId,
        Guid? orderId,
        Guid? listingId,
        FeedbackType feedbackType,
        FeedbackReason? reason,
        string? comment)
    {
        if (string.IsNullOrWhiteSpace(sellerId))
        {
            return Result.Failure<BuyerFeedbackEntity>(Error.Validation("BuyerFeedback.SellerId", "Seller ID is required"));
        }

        if (string.IsNullOrWhiteSpace(buyerId))
        {
            return Result.Failure<BuyerFeedbackEntity>(Error.Validation("BuyerFeedback.BuyerId", "Buyer ID is required"));
        }

        if (feedbackType == FeedbackType.Negative && !reason.HasValue)
        {
            return Result.Failure<BuyerFeedbackEntity>(Error.Validation("BuyerFeedback.Reason", "Negative feedback must include a reason"));
        }

        if (!string.IsNullOrEmpty(comment) && comment.Length > 1000)
        {
            return Result.Failure<BuyerFeedbackEntity>(Error.Validation("BuyerFeedback.Comment", "Comment cannot exceed 1000 characters"));
        }

        var feedback = new BuyerFeedbackEntity(Guid.NewGuid(), sellerId, buyerId, orderId, listingId, feedbackType, reason, comment);
        return Result.Success(feedback);
    }

    public void Update(FeedbackType feedbackType, FeedbackReason? reason, string? comment)
    {
        FeedbackType = feedbackType;
        Reason = reason;
        Comment = comment;
    }
}