using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentPayoutDetailDto(
    string PayoutId,
    DateTime PayoutDateUtc,
    string Status,
    string Account,
    string Memo,
    decimal Amount,
    string Currency,
    int TransactionCount,
    IReadOnlyList<PaymentPayoutTransactionDto> Transactions);
