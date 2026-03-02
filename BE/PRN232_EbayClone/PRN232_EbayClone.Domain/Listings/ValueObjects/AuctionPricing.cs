namespace PRN232_EbayClone.Domain.Listings.ValueObjects;

public record AuctionPricing(
    decimal StartPrice,
    decimal? ReservePrice,
    decimal? BuyItNowPrice,
    int Quantity = 1);
