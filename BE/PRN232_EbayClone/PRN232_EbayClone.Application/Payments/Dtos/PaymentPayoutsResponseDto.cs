using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentPayoutsResponseDto(
    IReadOnlyList<PaymentPayoutSummaryDto> Payouts,
    int TotalCount,
    decimal TotalAmount,
    string Currency);
