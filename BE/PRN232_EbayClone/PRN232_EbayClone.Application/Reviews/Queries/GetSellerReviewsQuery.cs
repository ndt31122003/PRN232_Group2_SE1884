using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Application.Reviews.Mappings;

namespace PRN232_EbayClone.Application.Reviews.Queries;

public sealed record GetSellerReviewsQuery(
    Guid SellerId,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int? Rating = null,
    int? MinRating = null,
    int? MaxRating = null,
    bool? IsReplied = null,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<SellerReviewsResult>;

public sealed record SellerReviewsResult(
    PagingResult<ReviewDto> Reviews,
    decimal AverageRating
);

public sealed class GetSellerReviewsQueryValidator : AbstractValidator<GetSellerReviewsQuery>
{
    public GetSellerReviewsQueryValidator()
    {
        RuleFor(x => x.SellerId).NotEmpty().WithMessage("Seller ID là bắt buộc");
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number phải lớn hơn 0");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200).WithMessage("Page size phải từ 1 đến 200");
        
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .When(x => x.Rating.HasValue)
            .WithMessage("Rating phải từ 1 đến 5");
            
        RuleFor(x => x.MinRating)
            .InclusiveBetween(1, 5)
            .When(x => x.MinRating.HasValue)
            .WithMessage("Min rating phải từ 1 đến 5");
            
        RuleFor(x => x.MaxRating)
            .InclusiveBetween(1, 5)
            .When(x => x.MaxRating.HasValue)
            .WithMessage("Max rating phải từ 1 đến 5");
            
        RuleFor(x => x)
            .Must(x => !x.MinRating.HasValue || !x.MaxRating.HasValue || x.MinRating <= x.MaxRating)
            .WithMessage("Min rating phải nhỏ hơn hoặc bằng max rating");
            
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date phải sau start date");
    }
}

public sealed class GetSellerReviewsQueryHandler : IQueryHandler<GetSellerReviewsQuery, SellerReviewsResult>
{
    private readonly IReviewRepository _reviewRepository;

    public GetSellerReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<SellerReviewsResult>> Handle(
        GetSellerReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var (reviews, totalCount, averageRating) = await _reviewRepository.GetSellerReviewsAsync(
            request.SellerId,
            request.StartDate,
            request.EndDate,
            request.Rating,
            request.MinRating,
            request.MaxRating,
            request.IsReplied,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var reviewDtos = reviews.Select(r => r.ToDto()).ToList();

        var pagingResult = new PagingResult<ReviewDto>(
            reviewDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        var result = new SellerReviewsResult(pagingResult, averageRating);

        return Result.Success(result);
    }
}