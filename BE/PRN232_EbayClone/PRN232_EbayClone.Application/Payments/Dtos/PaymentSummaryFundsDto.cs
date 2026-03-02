namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentSummaryFundsDto(
    decimal Available,
    decimal Processing,
    decimal OnHold,
    string Currency);
