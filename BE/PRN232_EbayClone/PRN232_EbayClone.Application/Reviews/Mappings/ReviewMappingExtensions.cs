using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Domain.Reviews.Entities;

namespace PRN232_EbayClone.Application.Reviews.Mappings;

internal static class ReviewMappingExtensions
{
    public static ReviewDto ToDto(this Review review)
    {
        // Note: ReviewerUsername and ReviewerFullName would need to be loaded from User table
        // For now, returning empty strings. In repository, we should include User navigation
        return new ReviewDto(
            review.Id,
            review.ListingId,
            review.ReviewerId,
            review.ReviewerRole.ToString(),
            string.Empty, // Would need to load from User
            string.Empty, // Would need to load from User
            review.RecipientId,
            review.RecipientRole.ToString(),
            review.Rating,
            review.RatingType.ToString(),
            review.Comment,
            review.Reply,
            review.RepliedAt,
            review.RevisionStatus.ToString(),
            review.RevisionRequestedAt,
            review.CreatedAt);
    }
}
