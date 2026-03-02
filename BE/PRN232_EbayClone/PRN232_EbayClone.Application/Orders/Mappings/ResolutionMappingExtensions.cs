using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Mappings;

internal static class ResolutionMappingExtensions
{
    public static PagingResult<CancellationSummaryDto> ToDto(
        this (IReadOnlyList<CancellationSummaryRecord> Records, int TotalCount) page,
        int pageNumber,
        int pageSize)
    {
        var items = page.Records.Select(ToDto).ToList();
        return new PagingResult<CancellationSummaryDto>(items, page.TotalCount, pageNumber, pageSize);
    }

    public static PagingResult<ReturnSummaryDto> ToDto(
        this (IReadOnlyList<ReturnRequestSummaryRecord> Records, int TotalCount) page,
        int pageNumber,
        int pageSize)
    {
        var items = page.Records.Select(ToReturnSummaryDto).ToList();
        return new PagingResult<ReturnSummaryDto>(items, page.TotalCount, pageNumber, pageSize);
    }

    public static CancellationSummaryDto ToDto(this CancellationSummaryRecord record)
    {
        var buyerDisplayName = string.IsNullOrWhiteSpace(record.BuyerFullName)
            ? record.BuyerUsername
            : record.BuyerFullName;

        var itemsSummary = BuildItemsSummary(record.ItemTitles, record.ItemCount);
        var (statusDisplay, statusCategory, actionLabel) = MapCancellationStatus(record.Status, record.InitiatedBy);
        var requiresAction = record.Status is CancellationStatus.PendingSellerResponse or CancellationStatus.PendingBuyerConfirmation;
        var details = BuildCancellationDetails(record.Reason, record.InitiatedBy);

        return new CancellationSummaryDto(
            record.RequestId,
            record.OrderId,
            record.OrderNumber,
            buyerDisplayName,
            record.BuyerUsername,
            record.ItemTitles,
            itemsSummary,
            statusDisplay,
            statusCategory,
            actionLabel,
            details,
            record.OrderTotalAmount,
            record.OrderTotalCurrency,
            record.RefundAmount,
            record.RefundCurrency,
            record.RequestedAtUtc,
            record.SellerResponseDeadlineUtc,
            record.SellerRespondedAtUtc,
            record.CompletedAtUtc,
            requiresAction,
            MapInitiator(record.InitiatedBy),
            MapCancellationReason(record.Reason),
            record.BuyerNote,
            record.SellerNote);
    }

    public static ReturnSummaryDto ToReturnSummaryDto(this ReturnRequestSummaryRecord record)
    {
        var buyerDisplayName = string.IsNullOrWhiteSpace(record.BuyerFullName)
            ? record.BuyerUsername
            : record.BuyerFullName;

        var itemsSummary = BuildItemsSummary(record.ItemTitles, record.ItemCount);
        var (statusDisplay, statusCategory, actionLabel) = MapReturnStatus(record.Status, record.PreferredResolution);
        var requiresAction = record.Status is ReturnStatus.PendingSellerResponse
            or ReturnStatus.DeliveredToSeller
            or ReturnStatus.RefundPending;

        var details = BuildReturnDetails(record.Reason, record.PreferredResolution);

        return new ReturnSummaryDto(
            record.RequestId,
            record.OrderId,
            record.OrderNumber,
            buyerDisplayName,
            record.BuyerUsername,
            record.ItemTitles,
            itemsSummary,
            statusDisplay,
            statusCategory,
            actionLabel,
            details,
            record.OrderTotalAmount,
            record.OrderTotalCurrency,
            record.RefundAmount,
            record.RefundCurrency,
            record.RestockingFeeAmount,
            record.RestockingFeeCurrency,
            record.RequestedAtUtc,
            record.SellerRespondedAtUtc,
            record.BuyerReturnDueAtUtc,
            record.BuyerShippedAtUtc,
            record.DeliveredAtUtc,
            record.RefundIssuedAtUtc,
            record.ClosedAtUtc,
            record.ReturnCarrier,
            record.TrackingNumber,
            requiresAction,
            MapReturnReason(record.Reason),
            MapResolution(record.PreferredResolution),
            record.BuyerNote,
            record.SellerNote,
            record.IsOrderPaid);
    }

    private static string BuildItemsSummary(IReadOnlyCollection<string> itemTitles, int itemCount)
    {
        if (itemTitles.Count == 0)
        {
            return "No items";
        }

        var firstTitle = itemTitles.First();
        if (itemCount <= 1)
        {
            return firstTitle;
        }

        return $"{firstTitle} +{itemCount - 1} more";
    }

    private static (string Display, string Category, string Action) MapCancellationStatus(
        CancellationStatus status,
        CancellationInitiator initiatedBy)
    {
        return status switch
        {
            CancellationStatus.PendingSellerResponse => ("Awaiting your response", "needs-attention", "Review request"),
            CancellationStatus.PendingBuyerConfirmation => ("Waiting on buyer", "in-progress", "Follow up"),
            CancellationStatus.AwaitingRefund => (initiatedBy == CancellationInitiator.Seller ? "Refund pending" : "Issue refund",
                initiatedBy == CancellationInitiator.Seller ? "in-progress" : "needs-attention",
                initiatedBy == CancellationInitiator.Seller ? "View details" : "Record refund"),
            CancellationStatus.Completed => ("Cancelled", "closed", "View details"),
            CancellationStatus.Declined => ("Declined", "closed", "View details"),
            CancellationStatus.AutoCancelled => ("Auto cancelled", "closed", "View details"),
            _ => ("Unknown", "unknown", "View details"),
        };
    }

    private static string MapInitiator(CancellationInitiator initiator)
        => initiator switch
        {
            CancellationInitiator.Buyer => "Buyer",
            CancellationInitiator.Seller => "Seller",
            CancellationInitiator.System => "System",
            _ => "Unknown"
        };

    private static string MapCancellationReason(CancellationReason reason)
        => reason switch
        {
            CancellationReason.BuyerRequest => "Buyer requested",
            CancellationReason.BuyerChangedMind => "Buyer changed mind",
            CancellationReason.BuyerUnpaid => "Buyer unpaid",
            CancellationReason.IncorrectAddress => "Incorrect address",
            CancellationReason.OutOfStock => "Out of stock",
            CancellationReason.PricingError => "Pricing error",
            CancellationReason.DuplicateOrder => "Duplicate order",
            CancellationReason.SuspectedFraud => "Suspected fraud",
            CancellationReason.Other => "Other",
            _ => "Other"
        };

    private static string BuildCancellationDetails(CancellationReason reason, CancellationInitiator initiator)
    {
        var initiatorText = MapInitiator(initiator);
        var reasonText = MapCancellationReason(reason);
        return $"{initiatorText} · {reasonText}";
    }

    private static (string Display, string Category, string Action) MapReturnStatus(
        ReturnStatus status,
        ReturnResolution resolution)
    {
        return status switch
        {
            ReturnStatus.PendingSellerResponse => ("Awaiting your response", "needs-attention", "Review request"),
            ReturnStatus.AwaitingBuyerReturn => ("Waiting for buyer to ship", "in-progress", "View details"),
            ReturnStatus.InTransitBackToSeller => ("Buyer shipped", "in-progress", "Track package"),
            ReturnStatus.DeliveredToSeller => ("Delivered to you", "needs-attention", "Process return"),
            ReturnStatus.RefundPending => ("Refund pending", "needs-attention", "Issue refund"),
            ReturnStatus.ReplacementSent => ("Replacement sent", "in-progress", "View details"),
            ReturnStatus.RefundCompleted => ("Refund completed", "closed", "View details"),
            ReturnStatus.SellerDeclined => ("Declined", "closed", "View details"),
            ReturnStatus.Closed => ("Closed", "closed", "View details"),
            _ => ("Unknown", "unknown", "View details")
        };
    }

    private static string MapReturnReason(ReturnReason reason)
        => reason switch
        {
            ReturnReason.NotAsDescribed => "Not as described",
            ReturnReason.DamagedOrDefective => "Damaged or defective",
            ReturnReason.WrongItemReceived => "Wrong item received",
            ReturnReason.MissingPartsOrAccessories => "Missing parts or accessories",
            ReturnReason.DoesNotFit => "Does not fit",
            ReturnReason.ArrivedLate => "Arrived late",
            ReturnReason.ChangedMind => "Changed mind",
            ReturnReason.OrderedByMistake => "Ordered by mistake",
            ReturnReason.NotReceived => "Item not received",
            ReturnReason.Other => "Other",
            _ => "Other"
        };

    private static string MapResolution(ReturnResolution resolution)
        => resolution switch
        {
            ReturnResolution.Refund => "Refund",
            ReturnResolution.Replacement => "Replacement",
            ReturnResolution.Exchange => "Exchange",
            ReturnResolution.PartialRefund => "Partial refund",
            ReturnResolution.Repair => "Repair",
            ReturnResolution.Other => "Other",
            _ => "Other"
        };

    private static string BuildReturnDetails(ReturnReason reason, ReturnResolution resolution)
    {
        var reasonText = MapReturnReason(reason);
        var resolutionText = MapResolution(resolution);
        return $"{reasonText} · {resolutionText}";
    }
}
