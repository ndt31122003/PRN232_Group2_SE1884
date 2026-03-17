namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeSummaryDto
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ListingId { get; init; }
    public string ListingTitle { get; init; } = string.Empty;
    public string DisputeType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime OpenedAt { get; init; }
    public DateTime? DeadlineAt { get; init; }
    public bool IsDeadlineApproaching { get; init; }
    public int MessageCount { get; init; }
}
