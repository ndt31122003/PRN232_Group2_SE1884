namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record CreateDisputeRequest
{
    public Guid OrderId { get; init; }
    public Guid ListingId { get; init; }
    public string DisputeType { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
}
