using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record CancellationSummaryRecord(
    Guid RequestId,
    Guid OrderId,
    string OrderNumber,
    DateTime OrderedAtUtc,
    DateTime RequestedAtUtc,
    DateTime? SellerRespondedAtUtc,
    DateTime? SellerResponseDeadlineUtc,
    DateTime? AutoClosedAtUtc,
    DateTime? CompletedAtUtc,
    CancellationStatus Status,
    CancellationInitiator InitiatedBy,
    CancellationReason Reason,
    string? BuyerNote,
    string? SellerNote,
    decimal OrderTotalAmount,
    string OrderTotalCurrency,
    decimal? RefundAmount,
    string? RefundCurrency,
    string BuyerUsername,
    string BuyerFullName,
    IReadOnlyCollection<string> ItemTitles,
    int ItemCount
);
