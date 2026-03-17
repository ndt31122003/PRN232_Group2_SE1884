namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record AcceptRefundRequest
{
    public decimal RefundAmount { get; init; }
    public string? Message { get; init; }
}
