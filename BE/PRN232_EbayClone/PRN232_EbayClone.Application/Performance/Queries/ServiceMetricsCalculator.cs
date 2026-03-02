using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Performance.Queries;

internal sealed class ServiceMetricsCalculator
{
    private static readonly Dictionary<string, string> ItemNotAsDescribedReasonMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MissingPartsOrAccessories"] = "missingParts",
        ["MissingParts"] = "missingParts",
        ["PartsMissing"] = "missingParts",
        ["ArrivedDamaged"] = "arrivedDamaged",
        ["DamagedOrDefective"] = "arrivedDamaged",
        ["Defective"] = "defective",
        ["NotWorkingOrDefective"] = "defective",
        ["NotWorking"] = "defective",
        ["WrongItemReceived"] = "wrongItem",
        ["WrongItem"] = "wrongItem",
        ["IncorrectItem"] = "wrongItem",
        ["NotAsDescribed"] = "notMatch",
        ["ItemNotAsDescribed"] = "notMatch",
        ["DoesNotMatchDescription"] = "notMatch",
        ["DoesNotMatchPhotos"] = "notMatch",
        ["NotAuthentic"] = "notAuthentic",
        ["Counterfeit"] = "notAuthentic"
    };

    private static readonly IReadOnlyList<string> OrderedReasonCategories = new[]
    {
        "missingParts",
        "arrivedDamaged",
        "defective",
        "wrongItem",
        "notMatch",
        "notAuthentic",
        "other"
    };

    private static readonly Dictionary<string, string> ReasonLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        { "missingParts", "Missing parts or pieces" },
        { "arrivedDamaged", "Arrived damaged" },
        { "defective", "Not working or defective" },
        { "wrongItem", "Wrong item" },
        { "notMatch", "Doesn't match description or photos" },
        { "notAuthentic", "Not authentic" },
        { "other", "Other reasons" }
    };

    private const decimal PeerBaselineItemNotAsDescribed = 0.75m;
    private const decimal PeerBaselineItemNotReceived = 0.29m;
    private const decimal MaxGaugeRate = 5m;

    private readonly IReadOnlyList<ServiceMetricsOrderRecord> _orders;
    private readonly IReadOnlyList<ServiceMetricsReturnRecord> _returns;
    private readonly IReadOnlyList<ServiceMetricsCancellationRecord> _cancellations;
    private readonly DateTime _rangeStartUtc;
    private readonly DateTime _rangeEndUtc;

    public ServiceMetricsCalculator(
        ServiceMetricsSourceRecord source,
        DateTime rangeStartUtc,
        DateTime rangeEndUtc)
    {
        _orders = source.Orders;
        _returns = source.Returns;
        _cancellations = source.Cancellations;
        _rangeStartUtc = rangeStartUtc;
        _rangeEndUtc = rangeEndUtc;
    }

    public ServiceMetricsSnapshot Compute()
    {
        var eligibleOrders = _orders
            .Where(o =>
                o.OrderedAtUtc >= _rangeStartUtc &&
                o.OrderedAtUtc <= _rangeEndUtc &&
                o.PaidAtUtc.HasValue)
            .ToList();

        var evaluationOrderIds = eligibleOrders
            .Select(o => o.OrderId)
            .Distinct()
            .ToHashSet();

        var itemNotAsDescribedMetric = BuildItemNotAsDescribedMetric(eligibleOrders, evaluationOrderIds);
        var itemNotReceivedMetric = BuildItemNotReceivedMetric(eligibleOrders, evaluationOrderIds);

        return new ServiceMetricsSnapshot(itemNotAsDescribedMetric, itemNotReceivedMetric);
    }

    private ServiceMetricDto BuildItemNotAsDescribedMetric(
        IReadOnlyList<ServiceMetricsOrderRecord> eligibleOrders,
        HashSet<Guid> evaluationOrderIds)
    {
        var breakdown = OrderedReasonCategories.ToDictionary(category => category, _ => 0);
        var issueOrderIds = new HashSet<Guid>();
        var issueBuyerIds = new HashSet<Guid>();

        foreach (var returnRecord in _returns)
        {
            if (!evaluationOrderIds.Contains(returnRecord.OrderId))
            {
                continue;
            }

            if (returnRecord.RequestedAtUtc < _rangeStartUtc || returnRecord.RequestedAtUtc > _rangeEndUtc)
            {
                continue;
            }

            if (!TryResolveItemNotAsDescribedCategory(returnRecord.Reason, out var categoryId))
            {
                continue;
            }

            issueOrderIds.Add(returnRecord.OrderId);
            issueBuyerIds.Add(returnRecord.BuyerId);

            if (!breakdown.ContainsKey(categoryId))
            {
                breakdown[categoryId] = 0;
            }

            breakdown[categoryId] += 1;
        }

        var totalTransactions = eligibleOrders.Count;
        var issueCount = issueOrderIds.Count;
        var rate = ComputeRate(issueCount, totalTransactions);
        var peerRate = ComputePeerRate(rate, PeerBaselineItemNotAsDescribed);
        var classification = DetermineClassification(rate);

        var reasonDtos = OrderedReasonCategories
            .Select(categoryId => new ServiceMetricReasonDto(
                categoryId,
                ReasonLabels.TryGetValue(categoryId, out var label) ? label : categoryId,
                breakdown.TryGetValue(categoryId, out var count) ? count : 0))
            .ToList();

        return new ServiceMetricDto(
            MetricId: "itemNotAsDescribed",
            TotalTransactions: totalTransactions,
            IssueCount: issueCount,
            Rate: rate,
            RateLabel: FormatPercent(rate),
            PeerRate: peerRate,
            PeerRateLabel: FormatPercent(peerRate),
            Classification: classification,
            UniqueBuyerCount: issueBuyerIds.Count,
            Reasons: reasonDtos);
    }

    private static bool TryResolveItemNotAsDescribedCategory(ReturnReason reason, out string categoryId)
    {
        var name = reason.ToString();
        if (ItemNotAsDescribedReasonMap.TryGetValue(name, out categoryId))
        {
            return true;
        }

        var normalized = NormalizeReasonText(name);
        if (string.IsNullOrEmpty(normalized))
        {
            categoryId = "other";
            return false;
        }

        if (IsBuyerPreferenceReturn(normalized))
        {
            categoryId = string.Empty;
            return false;
        }

        if (normalized.Contains("notreceived", StringComparison.Ordinal) ||
            normalized.Contains("neverreceived", StringComparison.Ordinal) ||
            normalized.Contains("notdelivered", StringComparison.Ordinal))
        {
            categoryId = string.Empty;
            return false;
        }

        if (normalized.Contains("missing", StringComparison.Ordinal) ||
            normalized.Contains("accessor", StringComparison.Ordinal))
        {
            categoryId = "missingParts";
            return true;
        }

        if (normalized.Contains("damage", StringComparison.Ordinal) ||
            normalized.Contains("broken", StringComparison.Ordinal) ||
            normalized.Contains("crack", StringComparison.Ordinal) ||
            normalized.Contains("dent", StringComparison.Ordinal) ||
            normalized.Contains("chip", StringComparison.Ordinal))
        {
            categoryId = "arrivedDamaged";
            return true;
        }

        if (normalized.Contains("defect", StringComparison.Ordinal) ||
            normalized.Contains("notwork", StringComparison.Ordinal) ||
            normalized.Contains("malfunction", StringComparison.Ordinal) ||
            normalized.Contains("faulty", StringComparison.Ordinal))
        {
            categoryId = "defective";
            return true;
        }

        if (normalized.Contains("wrong", StringComparison.Ordinal) ||
            normalized.Contains("incorrect", StringComparison.Ordinal) ||
            normalized.Contains("different", StringComparison.Ordinal))
        {
            categoryId = "wrongItem";
            return true;
        }

        if (normalized.Contains("description", StringComparison.Ordinal) ||
            normalized.Contains("match", StringComparison.Ordinal) ||
            normalized.Contains("notas", StringComparison.Ordinal))
        {
            categoryId = "notMatch";
            return true;
        }

        if (normalized.Contains("authentic", StringComparison.Ordinal) ||
            normalized.Contains("counterfeit", StringComparison.Ordinal) ||
            normalized.Contains("fake", StringComparison.Ordinal))
        {
            categoryId = "notAuthentic";
            return true;
        }

        categoryId = "other";
        return true;
    }

    private static string NormalizeReasonText(string? reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return string.Empty;
        }

        var buffer = new char[reason.Length];
        var index = 0;
        foreach (var character in reason)
        {
            if (!char.IsLetterOrDigit(character))
            {
                continue;
            }

            buffer[index++] = char.ToLowerInvariant(character);
        }

        return index == 0 ? string.Empty : new string(buffer, 0, index);
    }

    private static bool IsBuyerPreferenceReturn(string normalized)
    {
        if (string.IsNullOrEmpty(normalized))
        {
            return false;
        }

        return normalized.Contains("buyerremorse", StringComparison.Ordinal)
            || normalized.Contains("changedmind", StringComparison.Ordinal)
            || normalized.Contains("doesnotfit", StringComparison.Ordinal)
            || normalized.Contains("notfit", StringComparison.Ordinal)
            || normalized.Contains("wrongsize", StringComparison.Ordinal)
            || normalized.Contains("fits", StringComparison.Ordinal)
            || normalized.Contains("arrivedlate", StringComparison.Ordinal)
            || normalized.Contains("late", StringComparison.Ordinal);
    }

    private static bool IsItemNotReceivedReason(string normalized)
    {
        if (string.IsNullOrEmpty(normalized))
        {
            return false;
        }

        return normalized.Contains("notreceived", StringComparison.Ordinal)
            || normalized.Contains("neverreceived", StringComparison.Ordinal)
            || normalized.Contains("notdelivered", StringComparison.Ordinal)
            || normalized.Contains("neverarrived", StringComparison.Ordinal)
            || normalized.Contains("notarrive", StringComparison.Ordinal)
            || normalized.Contains("notarrived", StringComparison.Ordinal);
    }

    private ServiceMetricDto BuildItemNotReceivedMetric(
        IReadOnlyList<ServiceMetricsOrderRecord> eligibleOrders,
        HashSet<Guid> evaluationOrderIds)
    {
        var issueOrderIds = new HashSet<Guid>();
        var issueBuyerIds = new HashSet<Guid>();

        foreach (var returnRecord in _returns)
        {
            if (!evaluationOrderIds.Contains(returnRecord.OrderId))
            {
                continue;
            }

            if (returnRecord.RequestedAtUtc < _rangeStartUtc || returnRecord.RequestedAtUtc > _rangeEndUtc)
            {
                continue;
            }

            var normalized = NormalizeReasonText(returnRecord.Reason.ToString());
            if (!IsItemNotReceivedReason(normalized))
            {
                continue;
            }

            issueOrderIds.Add(returnRecord.OrderId);
            issueBuyerIds.Add(returnRecord.BuyerId);
        }

        foreach (var cancellation in _cancellations)
        {
            if (!evaluationOrderIds.Contains(cancellation.OrderId))
            {
                continue;
            }

            if (cancellation.RequestedAtUtc < _rangeStartUtc || cancellation.RequestedAtUtc > _rangeEndUtc)
            {
                continue;
            }

            if (cancellation.InitiatedBy != CancellationInitiator.Buyer)
            {
                continue;
            }

            issueOrderIds.Add(cancellation.OrderId);
            issueBuyerIds.Add(cancellation.BuyerId);
        }

        foreach (var order in eligibleOrders)
        {
            if (!string.Equals(order.StatusCode, OrderStatusCodes.DeliveryFailed, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            issueOrderIds.Add(order.OrderId);
            issueBuyerIds.Add(order.BuyerId);
        }

        var totalTransactions = eligibleOrders.Count;
        var issueCount = issueOrderIds.Count;
        var rate = ComputeRate(issueCount, totalTransactions);
        var peerRate = ComputePeerRate(rate, PeerBaselineItemNotReceived);
        var classification = DetermineClassification(rate);

        return new ServiceMetricDto(
            MetricId: "itemNotReceived",
            TotalTransactions: totalTransactions,
            IssueCount: issueCount,
            Rate: rate,
            RateLabel: FormatPercent(rate),
            PeerRate: peerRate,
            PeerRateLabel: FormatPercent(peerRate),
            Classification: classification,
            UniqueBuyerCount: issueBuyerIds.Count,
            Reasons: Array.Empty<ServiceMetricReasonDto>());
    }

    private static decimal ComputeRate(int issueCount, int totalTransactions)
    {
        if (totalTransactions == 0 || issueCount == 0)
        {
            return 0m;
        }

        var value = issueCount / (decimal)totalTransactions * 100m;
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal ComputePeerRate(decimal rate, decimal baseline)
    {
        if (rate <= baseline)
        {
            return baseline;
        }

        var adjusted = rate + 0.4m;
        if (adjusted < baseline)
        {
            adjusted = baseline;
        }

        return Math.Min(Math.Round(adjusted, 2, MidpointRounding.AwayFromZero), MaxGaugeRate);
    }

    private static string DetermineClassification(decimal rate) => rate switch
    {
        >= 2.5m => "very-high",
        >= 1.0m => "high",
        >= 0.3m => "average",
        _ => "low"
    };

    private static string FormatPercent(decimal value)
    {
        var numeric = value.ToString("0.00", CultureInfo.InvariantCulture);
        return string.Concat(numeric, "%");
    }
}

internal sealed record ServiceMetricsSnapshot(
    ServiceMetricDto ItemNotAsDescribed,
    ServiceMetricDto ItemNotReceived);