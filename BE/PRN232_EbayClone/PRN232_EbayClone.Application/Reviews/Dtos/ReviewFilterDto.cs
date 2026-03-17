namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReviewFilterDto
{
    public Guid? SellerId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int? MinStarRating { get; init; }
    public int? MaxStarRating { get; init; }
    public bool? HasReply { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SortBy { get; init; } = "CreatedAt";
    public string? SortOrder { get; init; } = "DESC";
}
