using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderPaymentRecord(
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
    string? BuyerFullName,
    string? BuyerUsername);
