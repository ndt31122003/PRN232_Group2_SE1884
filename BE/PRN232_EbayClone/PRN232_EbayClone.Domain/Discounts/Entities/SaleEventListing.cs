using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class SaleEventListing : Entity<Guid>
{
    public Guid SaleEventId { get; private set; }
    public Guid DiscountTierId { get; private set; }
    public Guid ListingId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    private SaleEventListing() : base(Guid.Empty) { }
    private SaleEventListing(Guid id) : base(id) { }

    public static SaleEventListing Create(
        Guid saleEventId,
        Guid discountTierId,
        Guid listingId)
    {
        return new SaleEventListing
        {
            Id = Guid.NewGuid(),
            SaleEventId = saleEventId,
            DiscountTierId = discountTierId,
            ListingId = listingId,
            AssignedAt = DateTime.UtcNow
        };
    }

    public void ReassignToTier(Guid newDiscountTierId)
    {
        DiscountTierId = newDiscountTierId;
        AssignedAt = DateTime.UtcNow;
    }
}
