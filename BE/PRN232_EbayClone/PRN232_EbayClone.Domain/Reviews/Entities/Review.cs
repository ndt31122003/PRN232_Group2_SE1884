using PRN232_EbayClone.Domain.Reviews.Enums;
using PRN232_EbayClone.Domain.Reviews.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Reviews.Entities;

public class Review : AggregateRoot<Guid>
{
    public Guid ListingId { get; private set; }
    public string ReviewerId { get; private set; } = null!;
    public string RecipientId { get; private set; } = null!;
    public ReviewParticipantRole ReviewerRole { get; private set; }
        = ReviewParticipantRole.Unknown;
    public ReviewParticipantRole RecipientRole { get; private set; }
        = ReviewParticipantRole.Unknown;
    public int Rating { get; private set; }
    public ReviewRatingType RatingType { get; private set; }
    public string? Comment { get; private set; }
    public string? Reply { get; private set; }
    public DateTime? RepliedAt { get; private set; }
    public ReviewRevisionStatus RevisionStatus { get; private set; }
    public DateTime? RevisionRequestedAt { get; private set; }

    // Navigation properties
    public Listing? Listing { get; private set; }

    private Review(Guid id) : base(id) { }

    public static Result<Review> Create(
        Guid listingId,
        string reviewerId,
        string recipientId,
        ReviewParticipantRole reviewerRole,
        ReviewParticipantRole recipientRole,
        int rating,
        string? comment = null)
    {
        if (string.IsNullOrWhiteSpace(reviewerId))
        {
            return Error.Validation("Review.ReviewerIdRequired", "Reviewer ID là bắt buộc");
        }

        if (string.IsNullOrWhiteSpace(recipientId))
        {
            return Error.Validation("Review.RecipientIdRequired", "Recipient ID là bắt buộc");
        }

        if (reviewerRole is ReviewParticipantRole.Unknown)
        {
            return Error.Validation("Review.ReviewerRoleRequired", "Phải xác định vai trò của người đánh giá");
        }

        if (recipientRole is ReviewParticipantRole.Unknown)
        {
            return Error.Validation("Review.RecipientRoleRequired", "Phải xác định vai trò của người nhận đánh giá");
        }

        if (rating < 1 || rating > 5)
        {
            return ReviewErrors.InvalidRating;
        }

        var ratingType = rating >= 4 ? ReviewRatingType.Positive
            : rating == 3 ? ReviewRatingType.Neutral
            : ReviewRatingType.Negative;

        var review = new Review(Guid.NewGuid())
        {
            ListingId = listingId,
            ReviewerId = reviewerId,
            ReviewerRole = reviewerRole,
            RecipientId = recipientId,
            RecipientRole = recipientRole,
            Rating = rating,
            RatingType = ratingType,
            Comment = comment,
            Reply = null,
            RepliedAt = null,
            RevisionStatus = ReviewRevisionStatus.None,
            RevisionRequestedAt = null
        };

        return Result.Success(review);
    }

    public Result UpdateComment(string? comment)
    {
        Comment = comment;
        return Result.Success();
    }

    public Result AddReply(string reply, Guid sellerId)
    {
        if (!string.IsNullOrWhiteSpace(Reply))
        {
            return ReviewErrors.AlreadyReplied;
        }

        if (string.IsNullOrWhiteSpace(reply))
        {
            return Error.Validation("Review.ReplyRequired", "Phản hồi không được để trống");
        }

        if (RecipientRole != ReviewParticipantRole.Seller)
        {
            return Error.Validation("Review.InvalidRecipientRole", "Chỉ phản hồi cho đánh giá dành cho người bán");
        }

        Reply = reply;
        RepliedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RequestRevision(Guid sellerId)
    {
        if (RevisionStatus == ReviewRevisionStatus.Pending)
        {
            return ReviewErrors.RevisionRequestExists;
        }

        if (RecipientRole != ReviewParticipantRole.Seller)
        {
            return Error.Validation("Review.InvalidRecipientRole", "Chỉ yêu cầu sửa với đánh giá dành cho người bán");
        }

        RevisionStatus = ReviewRevisionStatus.Pending;
        RevisionRequestedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result ApproveRevision()
    {
        if (RevisionStatus != ReviewRevisionStatus.Pending)
        {
            return Error.Failure("Review.InvalidRevisionStatus", "Trạng thái yêu cầu sửa đổi không hợp lệ");
        }

        RevisionStatus = ReviewRevisionStatus.Approved;

        return Result.Success();
    }

    public Result RejectRevision()
    {
        if (RevisionStatus != ReviewRevisionStatus.Pending)
        {
            return Error.Failure("Review.InvalidRevisionStatus", "Trạng thái yêu cầu sửa đổi không hợp lệ");
        }

        RevisionStatus = ReviewRevisionStatus.Rejected;

        return Result.Success();
    }
}
