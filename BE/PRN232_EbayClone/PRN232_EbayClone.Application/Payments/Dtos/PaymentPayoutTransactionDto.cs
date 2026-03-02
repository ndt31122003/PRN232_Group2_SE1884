using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentPayoutTransactionDto(
    Guid OrderId,
    string OrderNumber,
    string? BuyerUsername,
    string Status,
    DateTime OrderedAtUtc,
    DateTime? PaidAtUtc,
    decimal GrossAmount,
    decimal TotalFees,
    decimal NetAmount,
    string Currency);
