using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Performance.Dtos;

/// <summary>
/// Warning notification when seller metrics approach Below Standard thresholds.
/// </summary>
public sealed record SellerStandardsWarningDto(
    string Severity,              // "Critical" | "Warning" | "Info"
    string Title,                 // Display title
    string Summary,               // Brief summary
    IReadOnlyList<WarningMetric> Metrics,  // Specific metrics with warnings
    string ActionRequired,        // Action steps
    string NextEvaluationDate,    // When seller will be evaluated
    DateTime GeneratedAt          // When warning was generated
);

/// <summary>
/// Individual metric warning details.
/// </summary>
public sealed record WarningMetric(
    string MetricName,           // e.g., "Transaction Defect Rate"
    string CurrentValue,         // e.g., "1.8%"
    string Threshold,            // e.g., "2.0%"
    string Severity,             // "Critical" | "Warning" | "Info"
    string Message,              // User-friendly message
    string Recommendation        // Actionable recommendation
);