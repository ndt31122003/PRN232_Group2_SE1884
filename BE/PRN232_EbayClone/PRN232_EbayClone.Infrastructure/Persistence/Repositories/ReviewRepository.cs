using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Reviews.Enums;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository : Repository<Review, Guid>, IReviewRepository
{
    public ReviewRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Reviews
            .Include(r => r.Listing)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Review> Reviews, int TotalCount)> GetReviewsAsync(
        ReviewFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Reviews
            .AsNoTracking()
            .Include(r => r.Listing)
            .AsQueryable();

        if (filter.ListingId.HasValue)
        {
            query = query.Where(r => r.ListingId == filter.ListingId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ReviewerId))
        {
            query = query.Where(r => r.ReviewerId == filter.ReviewerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.ReviewerRole) &&
            Enum.TryParse<ReviewParticipantRole>(filter.ReviewerRole, ignoreCase: true, out var reviewerRole))
        {
            query = query.Where(r => r.ReviewerRole == reviewerRole);
        }

        if (!string.IsNullOrWhiteSpace(filter.RecipientId))
        {
            query = query.Where(r => r.RecipientId == filter.RecipientId);
        }

        if (!string.IsNullOrWhiteSpace(filter.RecipientRole) &&
            Enum.TryParse<ReviewParticipantRole>(filter.RecipientRole, ignoreCase: true, out var recipientRole))
        {
            query = query.Where(r => r.RecipientRole == recipientRole);
        }

        if (filter.Rating.HasValue)
        {
            query = query.Where(r => r.Rating == filter.Rating.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.RatingType))
        {
            if (Enum.TryParse<ReviewRatingType>(filter.RatingType, ignoreCase: true, out var ratingType))
            {
                query = query.Where(r => r.RatingType == ratingType);
            }
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerId))
        {
            query = query.Where(r =>
                (r.RecipientRole == ReviewParticipantRole.Seller && r.RecipientId == filter.SellerId) ||
                (r.Listing != null && r.Listing.CreatedBy == filter.SellerId));
        }

        if (!string.IsNullOrWhiteSpace(filter.RevisionStatus))
        {
            if (Enum.TryParse<ReviewRevisionStatus>(filter.RevisionStatus, ignoreCase: true, out var revisionStatus))
            {
                query = query.Where(r => r.RevisionStatus == revisionStatus);
            }
        }

        if (filter.HasReply.HasValue)
        {
            if (filter.HasReply.Value)
            {
                query = query.Where(r => !string.IsNullOrWhiteSpace(r.Reply));
            }
            else
            {
                query = query.Where(r => string.IsNullOrWhiteSpace(r.Reply));
            }
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= filter.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (reviews, totalCount);
    }

    public Task<IReadOnlyList<Review>> GetReviewsByListingIdAsync(
        Guid listingId,
        CancellationToken cancellationToken = default)
    {
        return DbContext.Reviews
            .AsNoTracking()
            .Where(r => r.ListingId == listingId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<Review>)t.Result);
    }
}
