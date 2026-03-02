using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentPayoutSummaryDto(
    string PayoutId,
    DateTime PayoutDateUtc,
    string Status,
    string Account,
    string Memo,
    int TransactionCount,
    decimal Amount,
    string Currency);
