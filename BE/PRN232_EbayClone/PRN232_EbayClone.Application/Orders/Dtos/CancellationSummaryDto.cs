using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record CancellationSummaryDto(
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
    DateTime RequestedAtUtc,
    DateTime? SellerResponseDeadlineUtc,
    DateTime? SellerRespondedAtUtc,
    DateTime? CompletedAtUtc,
    bool RequiresSellerAction,
    string InitiatedBy,
    string Reason,
    string? BuyerNote,
    string? SellerNote
);
