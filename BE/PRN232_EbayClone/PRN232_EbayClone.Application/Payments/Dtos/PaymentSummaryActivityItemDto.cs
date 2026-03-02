using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentSummaryActivityItemDto(
    Guid Id,
    DateTime OccurredAtUtc,
    string Type,
    string Description,
    string Status,
    decimal Amount,
    string Currency,
    string? OrderNumber,
    string? BuyerUsername);
