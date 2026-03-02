using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record BulkLeaveBuyerFeedbackItemResult(
    Guid OrderId,
    bool Success,
    LeaveBuyerFeedbackResult? Feedback,
    string? ErrorCode,
    string? ErrorMessage);

public sealed record BulkLeaveBuyerFeedbackResult(
    int SuccessCount,
    int FailureCount,
    IReadOnlyList<BulkLeaveBuyerFeedbackItemResult> Items);
