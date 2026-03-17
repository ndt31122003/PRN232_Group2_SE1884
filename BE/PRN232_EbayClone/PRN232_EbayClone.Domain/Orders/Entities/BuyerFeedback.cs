using System;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public sealed class BuyerFeedback : Entity<Guid>
{
    public const int MaxCommentLength = 80;

    public Guid OrderId { get; private set; }
    public Guid SellerId { get; private set; }
    public UserId BuyerId { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public bool UsesStoredComment { get; private set; }
    public string? StoredCommentKey { get; private set; }
    public int? StarRating { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? FollowUpComment { get; private set; }
    public DateTimeOffset? FollowUpCommentedAt { get; private set; }

    private BuyerFeedback() : base(Guid.Empty)
    {
    }

    private BuyerFeedback(
        Guid id,
        Guid orderId,
        Guid sellerId,
        UserId buyerId,
        string comment,
        bool usesStoredComment,
        string? storedCommentKey,
        DateTimeOffset createdAt) : base(id)
    {
        OrderId = orderId;
        SellerId = sellerId;
        BuyerId = buyerId;
        Comment = comment;
        UsesStoredComment = usesStoredComment;
        StoredCommentKey = storedCommentKey;
        CreatedAt = createdAt;
    }

    public static Result<BuyerFeedback> Create(
        Guid orderId,
        Guid sellerId,
        UserId buyerId,
        string comment,
        bool usesStoredComment,
        string? storedCommentKey,
        int? starRating,
        DateTimeOffset createdAt)
    {
        if (orderId == Guid.Empty)
        {
            return Error.Failure("Feedback.InvalidOrder", "Order id is required to leave feedback.");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Failure("Feedback.InvalidSeller", "Seller id is required to leave feedback.");
        }

        if (buyerId == default)
        {
            return Error.Failure("Feedback.InvalidBuyer", "Buyer id is required to leave feedback.");
        }

        var trimmedComment = comment?.Trim() ?? string.Empty;
        if (trimmedComment.Length == 0)
        {
            return Error.Failure("Feedback.InvalidComment", "Feedback comment cannot be empty.");
        }

        if (trimmedComment.Length > MaxCommentLength)
        {
            return Error.Failure(
                "Feedback.CommentTooLong",
                $"Feedback comment cannot exceed {MaxCommentLength} characters.");
        }

        if (starRating.HasValue && (starRating.Value < 1 || starRating.Value > 5))
        {
            return Error.Failure("Feedback.InvalidRating", "Star rating must be between 1 and 5.");
        }

        string? trimmedStoredKey = null;
        if (usesStoredComment)
        {
            trimmedStoredKey = storedCommentKey?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedStoredKey))
            {
                return Error.Failure("Feedback.StoredKeyRequired", "Stored feedback selection is required.");
            }
        }

        var feedback = new BuyerFeedback(
            Guid.NewGuid(),
            orderId,
            sellerId,
            buyerId,
            trimmedComment,
            usesStoredComment,
            trimmedStoredKey,
            createdAt);

        feedback.StarRating = starRating;

        return feedback;
    }

    public Result AddFollowUp(string comment, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            return Error.Failure("Feedback.InvalidFollowUp", "Follow-up comment cannot be empty.");
        }

        if (comment.Length > MaxCommentLength)
        {
            return Error.Failure(
                "Feedback.FollowUpTooLong",
                $"Follow-up comment cannot exceed {MaxCommentLength} characters.");
        }

        if (!string.IsNullOrWhiteSpace(FollowUpComment))
        {
            return Error.Failure("Feedback.FollowUpExists", "Follow-up feedback has already been left.");
        }

        FollowUpComment = comment.Trim();
        FollowUpCommentedAt = now;
        return Result.Success();
    }
}
