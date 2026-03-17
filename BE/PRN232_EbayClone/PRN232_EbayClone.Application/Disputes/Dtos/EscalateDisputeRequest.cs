namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record EscalateDisputeRequest
{
    public string Reason { get; init; } = string.Empty;
}
