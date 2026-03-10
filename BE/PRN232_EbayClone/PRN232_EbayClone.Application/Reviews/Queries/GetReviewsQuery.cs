using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Application.Reviews.Mappings;

namespace PRN232_EbayClone.Application.Reviews.Queries;

public sealed record GetReviewsQuery(
    ReviewFilterDto Filter
) : IQuery<PagingResult<ReviewDto>>;

public sealed class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(x => x.Filter).NotNull().WithMessage("Filter không được để trống");
        RuleFor(x => x.Filter.PageNumber).GreaterThan(0).WithMessage("Page number phải lớn hơn 0");
        RuleFor(x => x.Filter.PageSize).InclusiveBetween(1, 200).WithMessage("Page size phải từ 1 đến 200");
    }
}

public sealed class GetReviewsQueryHandler : IQueryHandler<GetReviewsQuery, PagingResult<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<PagingResult<ReviewDto>>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var (reviews, totalCount) = await _reviewRepository.GetReviewsAsync(
            request.Filter,
            cancellationToken);

        var reviewDtos = reviews.Select(r => r.ToDto()).ToList();

        var pagingResult = new PagingResult<ReviewDto>(
            reviewDtos,
            totalCount,
            request.Filter.PageNumber,
            request.Filter.PageSize);

        return Result.Success(pagingResult);
    }
}
