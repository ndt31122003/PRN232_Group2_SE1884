using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record ShippingLabelSummaryRecord(
    Guid LabelId,
    Guid OrderId,
    string OrderNumber,
    string Carrier,
    string ServiceName,
    string TrackingNumber,
    DateTimeOffset PurchasedAt,
    decimal CostAmount,
    string CostCurrency,
    string LabelUrl,
    string LabelFileName,
    bool IsVoided,
    DateTimeOffset? VoidedAt,
    string? VoidReason);
