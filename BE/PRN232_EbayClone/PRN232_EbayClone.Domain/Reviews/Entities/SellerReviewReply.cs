using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Domain.Reviews.Entities;

public sealed class SellerReviewReply : Entity<Guid>
{
    public const int MaxReplyLength = 500;

    public Guid ReviewId { get; private set; }
    public Guid SellerId { get; private set; }
    public string ReplyText { get; private set; } = string.Empty;
    public DateTimeOffset RepliedAt { get; private set; }
    public DateTimeOffset? EditedAt { get; private set; }
    public bool IsEdited { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public BuyerFeedback? Review { get; private set; }

    private SellerReviewReply() : base(Guid.Empty)
    {
    }

    private SellerReviewReply(
        Guid id,
        Guid reviewId,
        Guid sellerId,
        string replyText,
        DateTimeOffset repliedAt,
        string? createdBy) : base(id)
    {
        ReviewId = reviewId;
        SellerId = sellerId;
        ReplyText = replyText;
        RepliedAt = repliedAt;
        IsEdited = false;
        CreatedAt = repliedAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<SellerReviewReply> Create(
        Guid reviewId,
        Guid sellerId,
        string replyText,
        DateTimeOffset repliedAt,
        string? createdBy = null)
    {
        if (reviewId == Guid.Empty)
        {
            return Error.Validation("SellerReviewReply.InvalidReview", "Review ID is required");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Validation("SellerReviewReply.InvalidSeller", "Seller ID is required");
        }

        var trimmedReply = replyText?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedReply))
        {
            return Error.Validation("SellerReviewReply.ReplyRequired", "Reply text is required");
        }

        if (trimmedReply.Length > MaxReplyLength)
        {
            return Error.Validation(
                "SellerReviewReply.ReplyTooLong",
                $"Reply text cannot exceed {MaxReplyLength} characters");
        }

        var reply = new SellerReviewReply(
            Guid.NewGuid(),
            reviewId,
            sellerId,
            trimmedReply,
            repliedAt,
            createdBy);

        return reply;
    }

    public Result Edit(string newReplyText, DateTimeOffset editedAt, string? updatedBy = null)
    {
        if (IsDeleted)
        {
            return Error.Failure("SellerReviewReply.Deleted", "Cannot edit a deleted reply");
        }

        var trimmedReply = newReplyText?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedReply))
        {
            return Error.Validation("SellerReviewReply.ReplyRequired", "Reply text is required");
        }

        if (trimmedReply.Length > MaxReplyLength)
        {
            return Error.Validation(
                "SellerReviewReply.ReplyTooLong",
                $"Reply text cannot exceed {MaxReplyLength} characters");
        }

        ReplyText = trimmedReply;
        EditedAt = editedAt;
        IsEdited = true;
        UpdatedAt = editedAt;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }
}
