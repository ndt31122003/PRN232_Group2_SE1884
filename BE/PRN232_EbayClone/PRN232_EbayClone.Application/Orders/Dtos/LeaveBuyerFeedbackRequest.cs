namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record LeaveBuyerFeedbackRequest(
    bool UseStoredFeedback,
    string Comment,
    string? StoredFeedbackKey
);
