using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentPayoutsFilterDto
{
    public string Period { get; init; } = "last30Days";
    public DateTime? FromUtc { get; init; }
    public DateTime? ToUtc { get; init; }
    public string SearchBy { get; init; } = "payoutId";
    public string? Keyword { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
