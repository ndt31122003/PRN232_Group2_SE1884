/*
using MediatR;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.BuyerFeedback.Queries;

public record BuyerFeedbackDto(
    Guid Id,
    string BuyerId,
    string BuyerName,
    Guid? OrderId,
    Guid? ListingId,
    string? ListingTitle,
    FeedbackType FeedbackType,
    FeedbackReason? Reason,
    string? Comment,
    DateTime CreatedAt
);

public record GetBuyerFeedbacksQuery(
    FeedbackType? FeedbackType = null,
    string? BuyerId = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<Result<PagedResult<BuyerFeedbackDto>>>;

public class GetBuyerFeedbacksQueryHandler : IRequestHandler<GetBuyerFeedbacksQuery, Result<PagedResult<BuyerFeedbackDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetBuyerFeedbacksQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<PagedResult<BuyerFeedbackDto>>> Handle(GetBuyerFeedbacksQuery request, CancellationToken cancellationToken)
    {
        // Validate seller is authenticated
        if (string.IsNullOrEmpty(_currentUser.UserId))
        {
            return Result.Failure<PagedResult<BuyerFeedbackDto>>(Error.Unauthorized("Seller must be authenticated"));
        }

        var query = _context.BuyerFeedbacks
            .Where(bf => bf.SellerId == _currentUser.UserId && !bf.IsDeleted);

        // Apply filters
        if (request.FeedbackType.HasValue)
        {
            query = query.Where(bf => bf.FeedbackType == request.FeedbackType.Value);
        }

        if (!string.IsNullOrEmpty(request.BuyerId))
        {
            query = query.Where(bf => bf.BuyerId == request.BuyerId);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and get results
        var feedbacks = await query
            .OrderByDescending(bf => bf.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(bf => new BuyerFeedbackDto(
                bf.Id,
                bf.BuyerId,
                _context.Users.Where(u => u.Id == bf.BuyerId).Select(u => u.FullName).FirstOrDefault() ?? "Unknown",
                bf.OrderId,
                bf.ListingId,
                bf.ListingId.HasValue 
                    ? _context.Listings.Where(l => l.Id == bf.ListingId).Select(l => l.Title).FirstOrDefault()
                    : null,
                bf.FeedbackType,
                bf.Reason,
                bf.Comment,
                bf.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        var result = new PagedResult<BuyerFeedbackDto>(
            feedbacks,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result.Success(result);
    }
}
*/