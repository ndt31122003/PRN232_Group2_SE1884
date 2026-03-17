namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeFilterDto
{
    public Guid? SellerId { get; init; }
    public string? Status { get; init; }
    public string? DisputeType { get; init; }
    public bool? IsDeadlineApproaching { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SortBy { get; init; } = "OpenedAt";
    public string? SortOrder { get; init; } = "DESC";
}
