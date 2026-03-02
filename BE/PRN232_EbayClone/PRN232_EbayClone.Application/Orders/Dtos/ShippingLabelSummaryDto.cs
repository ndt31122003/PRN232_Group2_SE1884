using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record ShippingLabelSummaryDto(
    Guid LabelId,
    Guid OrderId,
    string OrderNumber,
    string Carrier,
    string ServiceName,
    string TrackingNumber,
    DateTimeOffset PurchasedAt,
    MoneyDto Cost,
    string LabelUrl,
    string LabelFileName,
    bool IsVoided,
    DateTimeOffset? VoidedAt,
    string? VoidReason);
