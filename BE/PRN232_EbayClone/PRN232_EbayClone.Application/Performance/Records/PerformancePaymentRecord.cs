using System;

namespace PRN232_EbayClone.Application.Performance.Records;

public sealed record PerformancePaymentRecord(
    Guid OrderId,
    string OrderNumber,
    DateTime OrderedAtUtc,
    DateTime? PaidAtUtc,
    string StatusCode,
    decimal SubTotalAmount,
    decimal ShippingAmount,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal PlatformFeeAmount,
    decimal TotalAmount,
    string Currency,
    Guid? BuyerId,
    string? BuyerFullName,
    string? BuyerUsername,
    decimal ShippingLabelAmount
);
