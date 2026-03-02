using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentTransactionsResponseDto(
    IReadOnlyList<PaymentTransactionDto> Transactions,
    decimal CurrentBalance,
    string Currency,
    IReadOnlyDictionary<string, int> SummaryCounts
);
