using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Domain.Reviews.Errors;
using Microsoft.EntityFrameworkCore;

namespace PRN232_EbayClone.Application.Reviews.Queries;

public sealed record GetReviewByIdQuery(
    Guid ReviewId
) : IQuery<ReviewDetailDto>;

public sealed class GetReviewByIdQueryValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdQueryValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty();
    }
}

public sealed class GetReviewByIdQueryHandler : IQueryHandler<GetReviewByIdQuery, ReviewDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public GetReviewByIdQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<ReviewDetailDto>> Handle(
        GetReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sellerId = Guid.Parse(_userContext.UserId!);

        var review = await _context.BuyerFeedbacks
            .Where(bf => bf.Id == request.ReviewId && bf.SellerId == sellerId)
            .Select(bf => new
            {
                Feedback = bf,
                Order = _context.Orders.FirstOrDefault(o => o.Id == bf.OrderId),
                Reply = _context.SellerReviewReplies
                    .Where(srr => srr.ReviewId == bf.Id && !srr.IsDeleted)
                    .FirstOrDefault(),
                Buyer = _context.Users.FirstOrDefault(u => u.Id == bf.BuyerId.Value)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (review == null)
        {
            return ReviewErrors.NotFound;
        }

        var dto = new ReviewDetailDto
        {
            Id = review.Feedback.Id,
            OrderId = review.Feedback.OrderId,
            OrderNumber = review.Order?.OrderNumber ?? "Unknown",
            ListingId = Guid.Empty, // TODO: Get from order items
            ListingTitle = "Unknown", // TODO: Get from order items
            BuyerId = review.Feedback.BuyerId.Value,
            BuyerUsername = review.Buyer?.Username ?? "Unknown",
            BuyerEmail = review.Buyer?.Email ?? "Unknown",
            StarRating = review.Feedback.StarRating ?? 0,
            Comment = review.Feedback.Comment,
            FollowUpComment = review.Feedback.FollowUpComment,
            CreatedAt = review.Feedback.CreatedAt.DateTime,
            FollowUpCommentedAt = review.Feedback.FollowUpCommentedAt?.DateTime,
            SellerReply = review.Reply != null ? new SellerReplyDto
            {
                Id = review.Reply.Id,
                ReplyText = review.Reply.ReplyText,
                RepliedAt = review.Reply.RepliedAt.DateTime,
                EditedAt = review.Reply.EditedAt?.DateTime,
                IsEdited = review.Reply.IsEdited
            } : null
        };

        return Result.Success(dto);
    }
}
