namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReviewListResponse
{
    public List<ReviewSummaryDto> Reviews { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public decimal AverageRating { get; init; }
}
