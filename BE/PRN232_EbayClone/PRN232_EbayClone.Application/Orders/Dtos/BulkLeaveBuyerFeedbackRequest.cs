using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record BulkLeaveBuyerFeedbackRequest(
    IReadOnlyList<Guid> OrderIds,
    bool UseStoredFeedback,
    string Comment,
    string? StoredFeedbackKey);
