using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentSummaryResponseDto(
    PaymentSummaryFundsDto Funds,
    PaymentSummaryScheduleDto Schedule,
    IReadOnlyList<PaymentSummaryActivityItemDto> RecentActivity);
