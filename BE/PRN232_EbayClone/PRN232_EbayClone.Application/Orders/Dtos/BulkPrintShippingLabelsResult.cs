using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record BulkShippingLabelItemResult(
    Guid OrderId,
    bool Success,
    PrintShippingLabelResult? Label,
    string? ErrorCode,
    string? ErrorMessage);

public sealed record BulkPrintShippingLabelsResult(
    int SuccessCount,
    int FailureCount,
    IReadOnlyList<BulkShippingLabelItemResult> Items);
