using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reviews.Dtos;
using Microsoft.EntityFrameworkCore;

namespace PRN232_EbayClone.Application.Reviews.Queries;

public sealed record GetSellerReviewsQuery(
    ReviewFilterDto Filter
) : IQuery<ReviewListResponse>;

public sealed class GetSellerReviewsQueryValidator : AbstractValidator<GetSellerReviewsQuery>
{
    public GetSellerReviewsQueryValidator()
    {
        RuleFor(x => x.Filter).NotNull();
        RuleFor(x => x.Filter.PageNumber).GreaterThan(0);
        RuleFor(x => x.Filter.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Filter.MinStarRating)
            .InclusiveBetween(1, 5)
            .When(x => x.Filter.MinStarRating.HasValue);
        RuleFor(x => x.Filter.MaxStarRating)
            .InclusiveBetween(1, 5)
            .When(x => x.Filter.MaxStarRating.HasValue);
        RuleFor(x => x.Filter.ToDate)
            .GreaterThan(x => x.Filter.FromDate!.Value)
            .When(x => x.Filter.FromDate.HasValue && x.Filter.ToDate.HasValue);
    }
}

public sealed class GetSellerReviewsQueryHandler : IQueryHandler<GetSellerReviewsQuery, ReviewListResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public GetSellerReviewsQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<ReviewListResponse>> Handle(
        GetSellerReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        
        // Use authenticated user's ID if SellerId not provided
        var sellerId = filter.SellerId ?? Guid.Parse(_userContext.UserId!);

        var query = _context.BuyerFeedbacks
            .Where(bf => bf.SellerId == sellerId && bf.StarRating.HasValue)
            .AsQueryable();

        // Apply filters
        if (filter.FromDate.HasValue)
        {
            query = query.Where(bf => bf.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(bf => bf.CreatedAt <= filter.ToDate.Value);
        }

        if (filter.MinStarRating.HasValue)
        {
            query = query.Where(bf => bf.StarRating >= filter.MinStarRating.Value);
        }

        if (filter.MaxStarRating.HasValue)
        {
            query = query.Where(bf => bf.StarRating <= filter.MaxStarRating.Value);
        }

        if (filter.HasReply.HasValue)
        {
            if (filter.HasReply.Value)
            {
                query = query.Where(bf => _context.SellerReviewReplies
                    .Any(srr => srr.ReviewId == bf.Id && !srr.IsDeleted));
            }
            else
            {
                query = query.Where(bf => !_context.SellerReviewReplies
                    .Any(srr => srr.ReviewId == bf.Id && !srr.IsDeleted));
            }
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Calculate average rating
        var averageRating = await query
            .Where(bf => bf.StarRating.HasValue)
            .AverageAsync(bf => (decimal)bf.StarRating!.Value, cancellationToken);

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "rating" => filter.SortOrder?.ToUpper() == "ASC"
                ? query.OrderBy(bf => bf.StarRating)
                : query.OrderByDescending(bf => bf.StarRating),
            "createdat" or _ => filter.SortOrder?.ToUpper() == "ASC"
                ? query.OrderBy(bf => bf.CreatedAt)
                : query.OrderByDescending(bf => bf.CreatedAt)
        };

        // Apply pagination
        var reviews = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(bf => new
            {
                Feedback = bf,
                Reply = _context.SellerReviewReplies
                    .Where(srr => srr.ReviewId == bf.Id && !srr.IsDeleted)
                    .FirstOrDefault(),
                Buyer = _context.Users.FirstOrDefault(u => u.Id == bf.BuyerId.Value)
            })
            .ToListAsync(cancellationToken);

        var reviewDtos = reviews.Select(r => new ReviewSummaryDto
        {
            Id = r.Feedback.Id,
            OrderId = r.Feedback.OrderId,
            ListingId = Guid.Empty, // TODO: Get from order items
            BuyerId = r.Feedback.BuyerId.Value,
            BuyerUsername = r.Buyer?.Username ?? "Unknown",
            StarRating = r.Feedback.StarRating ?? 0,
            Comment = r.Feedback.Comment,
            CreatedAt = r.Feedback.CreatedAt.DateTime,
            SellerReply = r.Reply?.ReplyText,
            RepliedAt = r.Reply?.RepliedAt.DateTime,
            HasReply = r.Reply != null
        }).ToList();

        return Result.Success(new ReviewListResponse
        {
            Reviews = reviewDtos,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            AverageRating = averageRating
        });
    }
}
