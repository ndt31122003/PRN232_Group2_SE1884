using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Performance.Records;

public sealed record ServiceMetricsOrderRecord(
    Guid OrderId,
    Guid BuyerId,
    DateTime OrderedAtUtc,
    DateTime? PaidAtUtc,
    DateTime? ShippedAtUtc,
    DateTime? DeliveredAtUtc,
    DateTime? CancelledAtUtc,
    string StatusCode,
    decimal TotalAmount,
    string Currency);

public sealed record ServiceMetricsReturnRecord(
    Guid RequestId,
    Guid OrderId,
    Guid BuyerId,
    ReturnReason Reason,
    ReturnStatus Status,
    DateTime RequestedAtUtc,
    DateTime? ClosedAtUtc);

public sealed record ServiceMetricsCancellationRecord(
    Guid RequestId,
    Guid OrderId,
    Guid BuyerId,
    CancellationInitiator InitiatedBy,
    CancellationReason Reason,
    CancellationStatus Status,
    DateTime RequestedAtUtc,
    DateTime? CompletedAtUtc);

public sealed record ServiceMetricsSourceRecord(
    IReadOnlyList<ServiceMetricsOrderRecord> Orders,
    IReadOnlyList<ServiceMetricsReturnRecord> Returns,
    IReadOnlyList<ServiceMetricsCancellationRecord> Cancellations);