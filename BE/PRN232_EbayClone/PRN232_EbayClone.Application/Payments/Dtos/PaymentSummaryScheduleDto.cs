using System;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentSummaryScheduleDto(
    string Account,
    string Frequency,
    DateTime? LastPayoutUtc,
    decimal LastPayoutAmount,
    DateTime? NextPayoutUtc,
    string Currency);
