using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentTransactionsFilterDto
{
    private const int DefaultPeriodDays = 90;

    public string Status { get; init; } = "all";
    public string Type { get; init; } = "all";
    public int PeriodDays { get; init; } = DefaultPeriodDays;
    public string SearchField { get; init; } = "orderNumber";
    public string? Search { get; init; }
}
