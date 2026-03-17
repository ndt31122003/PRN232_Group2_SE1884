namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeListResponse
{
    public List<DisputeSummaryDto> Disputes { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int WaitingSellerCount { get; init; }
    public int DeadlineApproachingCount { get; init; }
}
