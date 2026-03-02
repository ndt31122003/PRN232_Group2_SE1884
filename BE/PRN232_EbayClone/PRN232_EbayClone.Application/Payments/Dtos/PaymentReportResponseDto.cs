namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentReportResponseDto(
    PaymentReportPeriodDto Current,
    PaymentReportPeriodDto? Comparison);
