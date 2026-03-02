using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Performance.Queries;

/// <summary>
/// Check if seller is approaching Below Standard thresholds and needs warnings.
/// eBay sends warnings when sellers are within 10% of Below Standard limits.
/// </summary>
public sealed record CheckSellerStandardsWarningQuery(Guid SellerId) 
    : IQuery<SellerStandardsWarningDto?>;

public sealed class CheckSellerStandardsWarningQueryHandler(
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<CheckSellerStandardsWarningQuery, SellerStandardsWarningDto?>
{
    // eBay Below Standard thresholds
    private const decimal BelowStandardDefectRate = 2.0m;
    private const decimal BelowStandardLateShipmentRate = 4.0m;
    private const decimal BelowStandardTrackingRate = 90.0m;
    private const decimal BelowStandardCasesClosedRate = 1.0m;

    // Warning threshold: 10% away from Below Standard
    private const decimal WarningMarginPercent = 10m;

    public async Task<Result<SellerStandardsWarningDto?>> Handle(
        CheckSellerStandardsWarningQuery request,
        CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;
        var record = await PerformanceRepository.GetSellerLevelAsync(
            request.SellerId,
            nowUtc,
            cancellationToken);

        var warnings = new List<WarningMetric>();

        // 1. Check Transaction Defect Rate
        CheckDefectRateWarning(record.TransactionDefectRate, warnings);

        // 2. Check Late Shipment Rate
        CheckLateShipmentWarning(record.LateShipmentRate, warnings);

        // 3. Check Tracking Rate
        CheckTrackingRateWarning(record.TrackingUploadedOnTimeRate, warnings);

        // 4. Check Cases Closed Rate
        var casesClosedRate = record.TransactionsLast12Months > 0
            ? (record.CasesClosedWithoutSellerResolution / (decimal)record.TransactionsLast12Months * 100m)
            : 0m;
        CheckCasesClosedWarning(casesClosedRate, warnings);

        // No warnings needed
        if (warnings.Count == 0)
        {
            return Result.Success<SellerStandardsWarningDto?>(null);
        }

        // Determine overall severity
        var severity = warnings.Any(w => w.Severity == "Critical") 
            ? "Critical" 
            : warnings.Any(w => w.Severity == "Warning") 
                ? "Warning" 
                : "Info";

        var dto = new SellerStandardsWarningDto(
            Severity: severity,
            Title: BuildWarningTitle(severity, warnings.Count),
            Summary: BuildWarningSummary(warnings),
            Metrics: warnings,
            ActionRequired: BuildActionRequired(warnings),
            NextEvaluationDate: record.NextEvaluationDate.ToString("MMM d, yyyy", CultureInfo.InvariantCulture),
            GeneratedAt: nowUtc);

        return Result.Success<SellerStandardsWarningDto?>(dto);
    }

    private static void CheckDefectRateWarning(decimal currentRate, List<WarningMetric> warnings)
    {
        // Critical: >= 1.8% (within 10% of 2.0% threshold)
        if (currentRate >= 1.8m && currentRate < BelowStandardDefectRate)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Transaction Defect Rate",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardDefectRate:0.00}%",
                Severity: "Critical",
                Message: "Your defect rate is critically close to Below Standard threshold.",
                Recommendation: "Review recent defects: returns, cancellations, and delivery failures. Improve listing accuracy and product quality."
            ));
        }
        // Warning: >= 1.5% (within 25% of threshold)
        else if (currentRate >= 1.5m && currentRate < 1.8m)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Transaction Defect Rate",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardDefectRate:0.00}%",
                Severity: "Warning",
                Message: "Your defect rate is approaching Below Standard levels.",
                Recommendation: "Monitor returns and cancellations closely. Ensure accurate item descriptions."
            ));
        }
    }

    private static void CheckLateShipmentWarning(decimal currentRate, List<WarningMetric> warnings)
    {
        // Critical: >= 3.6% (within 10% of 4.0%)
        if (currentRate >= 3.6m && currentRate < BelowStandardLateShipmentRate)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Late Shipment Rate",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardLateShipmentRate:0.00}%",
                Severity: "Critical",
                Message: "Your late shipment rate is critically close to Below Standard threshold.",
                Recommendation: "Ship all orders within handling time (typically 2 business days). Consider same-day shipping."
            ));
        }
        // Warning: >= 3.0%
        else if (currentRate >= 3.0m && currentRate < 3.6m)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Late Shipment Rate",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardLateShipmentRate:0.00}%",
                Severity: "Warning",
                Message: "Your late shipment rate is approaching Below Standard levels.",
                Recommendation: "Improve shipping speed. Upload tracking within 1 day of shipment."
            ));
        }
    }

    private static void CheckTrackingRateWarning(decimal currentRate, List<WarningMetric> warnings)
    {
        // Critical: <= 91% (within 1% of 90% threshold)
        if (currentRate > 0 && currentRate <= 91m && currentRate > BelowStandardTrackingRate)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Tracking Uploaded On Time",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardTrackingRate:0.00}%",
                Severity: "Critical",
                Message: "Your tracking upload rate is critically close to Below Standard threshold.",
                Recommendation: "Upload valid tracking numbers within handling time for all shipments."
            ));
        }
        // Warning: <= 92%
        else if (currentRate > 0 && currentRate <= 92m && currentRate > 91m)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Tracking Uploaded On Time",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardTrackingRate:0.00}%",
                Severity: "Warning",
                Message: "Your tracking upload rate is approaching Below Standard levels.",
                Recommendation: "Ensure tracking is uploaded within 1 day of shipment. Use carrier-validated tracking."
            ));
        }
    }

    private static void CheckCasesClosedWarning(decimal currentRate, List<WarningMetric> warnings)
    {
        // Critical: >= 0.9% (within 10% of 1.0%)
        if (currentRate >= 0.9m && currentRate < BelowStandardCasesClosedRate)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Cases Closed Without Seller Resolution",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardCasesClosedRate:0.00}%",
                Severity: "Critical",
                Message: "Your cases closed rate is critically close to Below Standard threshold.",
                Recommendation: "Respond to all buyer messages within 3 business days. Resolve issues proactively."
            ));
        }
        // Warning: >= 0.75%
        else if (currentRate >= 0.75m && currentRate < 0.9m)
        {
            warnings.Add(new WarningMetric(
                MetricName: "Cases Closed Without Seller Resolution",
                CurrentValue: $"{currentRate:0.00}%",
                Threshold: $"{BelowStandardCasesClosedRate:0.00}%",
                Severity: "Warning",
                Message: "Your cases closed rate is approaching Below Standard levels.",
                Recommendation: "Improve response time to buyer issues. Offer refunds/replacements proactively."
            ));
        }
    }

    private static string BuildWarningTitle(string severity, int warningCount)
    {
        return severity switch
        {
            "Critical" => $"?? URGENT: {warningCount} metric(s) critically close to Below Standard",
            "Warning" => $"? Attention: {warningCount} metric(s) approaching Below Standard",
            _ => $"?? Performance Notice: {warningCount} metric(s) to monitor"
        };
    }

    private static string BuildWarningSummary(List<WarningMetric> warnings)
    {
        var metricNames = string.Join(", ", warnings.Select(w => w.MetricName));
        return $"The following metrics need attention: {metricNames}. " +
               "Take action now to avoid Below Standard status at your next evaluation.";
    }

    private static string BuildActionRequired(List<WarningMetric> warnings)
    {
        var criticalWarnings = warnings.Where(w => w.Severity == "Critical").ToList();
        
        if (criticalWarnings.Count > 0)
        {
            return "IMMEDIATE ACTION REQUIRED: Review the recommendations below and implement changes within 7 days. " +
                   "Failure to improve may result in Below Standard status.";
        }

        return "Monitor these metrics closely and follow recommendations to maintain Above Standard or Top Rated status.";
    }
}