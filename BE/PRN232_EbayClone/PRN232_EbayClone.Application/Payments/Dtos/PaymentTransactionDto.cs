using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentTransactionDto(
    Guid Id,
    DateTime OccurredAt,
    string Type,
    string Description,
    string? OrderNumber,
    string Status,
    string Bucket,
    string Currency,
    decimal Amount,
    decimal BalanceImpact,
    string? Buyer,
    string? ItemId,
    string? CaseId,
    string? TrackingNumber
);
