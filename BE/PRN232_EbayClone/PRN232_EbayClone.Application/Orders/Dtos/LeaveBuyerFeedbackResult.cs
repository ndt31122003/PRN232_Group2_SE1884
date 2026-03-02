namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record LeaveBuyerFeedbackResult(
    SellerFeedbackDto Feedback,
    OrderStatusUpdateResult Status
);
