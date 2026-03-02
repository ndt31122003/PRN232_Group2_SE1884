using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record ReturnSummaryDto(
    Guid RequestId,
    Guid OrderId,
    string OrderNumber,
    string BuyerDisplayName,
    string BuyerUsername,
    IReadOnlyCollection<string> ItemTitles,
    string ItemsSummary,
    string StatusDisplay,
    string StatusCategory,
    string ActionLabel,
    string Details,
    decimal OrderTotalAmount,
    string OrderTotalCurrency,
    decimal? RefundAmount,
    string? RefundCurrency,
    decimal? RestockingFeeAmount,
    string? RestockingFeeCurrency,
    DateTime RequestedAtUtc,
    DateTime? SellerRespondedAtUtc,
    DateTime? BuyerReturnDueAtUtc,
    DateTime? BuyerShippedAtUtc,
    DateTime? DeliveredAtUtc,
    DateTime? RefundIssuedAtUtc,
    DateTime? ClosedAtUtc,
    string? ReturnCarrier,
    string? TrackingNumber,
    bool RequiresSellerAction,
    string Reason,
    string PreferredResolution,
    string? BuyerNote,
    string? SellerNote,
    bool IsOrderPaid
);
