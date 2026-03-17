namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record AddDisputeMessageRequest
{
    public string Message { get; init; } = string.Empty;
}
