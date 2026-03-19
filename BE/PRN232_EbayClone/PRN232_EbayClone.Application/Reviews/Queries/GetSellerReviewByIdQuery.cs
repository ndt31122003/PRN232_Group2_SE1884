using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Application.Reviews.Mappings;

namespace PRN232_EbayClone.Application.Reviews.Queries;

public sealed record GetSellerReviewByIdQuery(
    Guid ReviewId,
    Guid SellerId
) : IQuery<ReviewDto>;

public sealed class GetSellerReviewByIdQueryValidator : AbstractValidator<GetSellerReviewByIdQuery>
{
    public GetSellerReviewByIdQueryValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID là bắt buộc");
        RuleFor(x => x.SellerId).NotEmpty().WithMessage("Seller ID là bắt buộc");
    }
}

public sealed class GetSellerReviewByIdQueryHandler : IQueryHandler<GetSellerReviewByIdQuery, ReviewDto>
{
    private readonly IReviewRepository _reviewRepository;

    public GetSellerReviewByIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<ReviewDto>> Handle(
        GetSellerReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetSellerReviewByIdAsync(
            request.ReviewId,
            request.SellerId,
            cancellationToken);

        if (review is null)
        {
            return Error.Failure("Review.NotFound", "Không tìm thấy review hoặc review không thuộc về seller này");
        }

        return Result.Success(review.ToDto());
    }
}