namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeDetailDto
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ListingId { get; init; }
    public string ListingTitle { get; init; } = string.Empty;
    public Guid BuyerId { get; init; }
    public string BuyerUsername { get; init; } = string.Empty;
    public Guid SellerId { get; init; }
    public string SellerUsername { get; init; } = string.Empty;
    public string DisputeType { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? ResolutionType { get; init; }
    public decimal? RefundAmount { get; init; }
    public string? RefundCurrency { get; init; }
    public DateTime OpenedAt { get; init; }
    public DateTime? ResolvedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public DateTime? EscalatedAt { get; init; }
    public DateTime? DeadlineAt { get; init; }
    public bool IsDeadlineApproaching { get; init; }
    public List<DisputeMessageDto> Messages { get; init; } = [];
    public List<DisputeEvidenceDto> Evidence { get; init; } = [];
    public List<DisputeStatusHistoryDto> StatusHistory { get; init; } = [];
}
