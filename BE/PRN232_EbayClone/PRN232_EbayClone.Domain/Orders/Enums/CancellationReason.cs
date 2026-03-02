namespace PRN232_EbayClone.Domain.Orders.Enums;

public enum CancellationReason
{
    BuyerRequest = 0,
    BuyerChangedMind = 1,
    BuyerUnpaid = 2,
    IncorrectAddress = 3,
    OutOfStock = 4,
    PricingError = 5,
    DuplicateOrder = 6,
    SuspectedFraud = 7,
    Other = 99
}
