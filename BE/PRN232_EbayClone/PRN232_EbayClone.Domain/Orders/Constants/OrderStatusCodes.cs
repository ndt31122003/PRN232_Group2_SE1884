namespace PRN232_EbayClone.Domain.Orders.Constants;

public static class OrderStatusCodes
{
    public const string Draft = "Draft";
    public const string AwaitingPayment = "AwaitingPayment";
    public const string AwaitingShipment = "AwaitingShipment";
    public const string AwaitingShipmentOverdue = "AwaitingShipmentOverdue";
    public const string AwaitingShipmentShipWithin24h = "AwaitingShipmentShipWithin24h";
    public const string AwaitingExpeditedShipment = "AwaitingExpeditedShipment";
    public const string PaidAndShipped = "PaidAndShipped";
    public const string PaidAwaitingFeedback = "PaidAwaitingFeedback";
    public const string ShippedAwaitingFeedback = "ShippedAwaitingFeedback";
    public const string DeliveryFailed = "DeliveryFailed";
    public const string Archived = "Archived";
    public const string Cancelled = "Cancelled";
}
