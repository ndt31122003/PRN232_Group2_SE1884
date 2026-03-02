using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record ReturnRequestSummaryRecord(
    Guid RequestId,
    Guid OrderId,
    string OrderNumber,
    DateTime OrderedAtUtc,
    DateTime RequestedAtUtc,
    DateTime? SellerRespondedAtUtc,
    DateTime? BuyerReturnDueAtUtc,
    DateTime? BuyerShippedAtUtc,
    DateTime? DeliveredAtUtc,
    DateTime? RefundIssuedAtUtc,
    DateTime? ClosedAtUtc,
    ReturnStatus Status,
    ReturnReason Reason,
    ReturnResolution PreferredResolution,
    string? BuyerNote,
    string? SellerNote,
    string? ReturnCarrier,
    string? TrackingNumber,
    decimal OrderTotalAmount,
    string OrderTotalCurrency,
    decimal? RefundAmount,
    string? RefundCurrency,
    decimal? RestockingFeeAmount,
    string? RestockingFeeCurrency,
    string BuyerUsername,
    string BuyerFullName,
    IReadOnlyCollection<string> ItemTitles,
    int ItemCount,
    bool IsOrderPaid
);
