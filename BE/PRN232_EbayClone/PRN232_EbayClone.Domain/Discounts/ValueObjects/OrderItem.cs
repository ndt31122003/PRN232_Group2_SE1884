using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.ValueObjects;

/// <summary>
/// Represents an order item for discount calculation purposes
/// </summary>
public sealed record OrderItem(
    Guid ListingId,
    Guid CategoryId,
    Money Price,
    int Quantity,
    DateTime LastPriceChange);
