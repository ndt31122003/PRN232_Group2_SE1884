namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentReportPeriodDto(
    DateTime StartUtc,
    DateTime EndUtc,
    string Currency,
    IReadOnlyDictionary<string, PaymentReportSectionDto> Sections);
