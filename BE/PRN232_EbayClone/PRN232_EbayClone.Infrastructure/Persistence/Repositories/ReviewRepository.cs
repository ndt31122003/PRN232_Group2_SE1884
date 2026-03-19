using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Reviews.Enums;
using PRN232_EbayClone.Domain.Listings.Entities;

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

    public async Task<(IReadOnlyList<Review> Reviews, int TotalCount, decimal AverageRating)> GetSellerReviewsAsync(
        Guid sellerId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? rating = null,
        int? minRating = null,
        int? maxRating = null,
        bool? isReplied = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var sellerUserId = new PRN232_EbayClone.Domain.Users.ValueObjects.UserId(sellerId);
        
        // Query reviews for seller using the listing_id mapping table
        var query = from review in DbContext.Reviews
                    join listingMapping in DbContext.ListingIdMappings 
                        on review.ListingId equals listingMapping.ListingId
                    where listingMapping.SellerId == sellerUserId
                    select review;

        // Apply filters
        if (startDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= endDate.Value);
        }

        if (rating.HasValue)
        {
            query = query.Where(r => r.Rating == rating.Value);
        }

        if (minRating.HasValue)
        {
            query = query.Where(r => r.Rating >= minRating.Value);
        }

        if (maxRating.HasValue)
        {
            query = query.Where(r => r.Rating <= maxRating.Value);
        }

        if (isReplied.HasValue)
        {
            if (isReplied.Value)
            {
                query = query.Where(r => !string.IsNullOrWhiteSpace(r.Reply));
            }
            else
            {
                query = query.Where(r => string.IsNullOrWhiteSpace(r.Reply));
            }
        }

        // Calculate average rating for filtered results
        var averageRating = await query.AverageAsync(r => (decimal)r.Rating, cancellationToken);
        if (double.IsNaN((double)averageRating))
        {
            averageRating = 0;
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting: priority to rating <= 3 + reply IS NULL first
        var reviews = await query
            .OrderBy(r => r.Rating <= 3 && string.IsNullOrWhiteSpace(r.Reply) ? 0 : 1)
            .ThenByDescending(r => r.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Listing)
            .ToListAsync(cancellationToken);

        return (reviews, totalCount, averageRating);
    }

    public async Task<Review?> GetSellerReviewByIdAsync(
        Guid reviewId,
        Guid sellerId,
        CancellationToken cancellationToken = default)
    {
        var sellerUserId = new PRN232_EbayClone.Domain.Users.ValueObjects.UserId(sellerId);
        
        // Validate review belongs to seller using the listing_id mapping table
        var review = await (from r in DbContext.Reviews
                           join listingMapping in DbContext.ListingIdMappings 
                               on r.ListingId equals listingMapping.ListingId
                           where r.Id == reviewId && listingMapping.SellerId == sellerUserId
                           select r)
                           .Include(r => r.Listing)
                           .SingleOrDefaultAsync(cancellationToken);

        return review;
    }
}
