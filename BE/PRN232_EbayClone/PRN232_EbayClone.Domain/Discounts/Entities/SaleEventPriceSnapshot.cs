using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class SaleEventPriceSnapshot : Entity<Guid>
{
    public Guid SaleEventId { get; private set; }
    public Guid ListingId { get; private set; }
    public decimal OriginalPrice { get; private set; }
    public DateTime SnapshotAt { get; private set; }

    private SaleEventPriceSnapshot() : base(Guid.Empty) { }
    private SaleEventPriceSnapshot(Guid id) : base(id) { }

    public static SaleEventPriceSnapshot Create(
        Guid saleEventId,
        Guid listingId,
        decimal originalPrice)
    {
        if (originalPrice <= 0)
            throw new ArgumentException("Original price must be greater than 0", nameof(originalPrice));

        return new SaleEventPriceSnapshot
        {
            Id = Guid.NewGuid(),
            SaleEventId = saleEventId,
            ListingId = listingId,
            OriginalPrice = originalPrice,
            SnapshotAt = DateTime.UtcNow
        };
    }
}
