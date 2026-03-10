using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SaleEvents.Entities;

public sealed class SaleEventListing : Entity<Guid>
{
    public Guid SaleEventId { get; private set; }
    public Guid DiscountTierId { get; private set; }
    public Guid ListingId { get; private set; }

    private SaleEventListing(Guid id) : base(id)
    {
    }

    public static Result<SaleEventListing> Create(Guid saleEventId, Guid discountTierId, Guid listingId)
    {
        if (listingId == Guid.Empty)
        {
            return Error.Validation(
                "SaleEventListing.InvalidListingId",
                "Listing id is required.");
        }

        var entity = new SaleEventListing(Guid.NewGuid())
        {
            SaleEventId = saleEventId,
            DiscountTierId = discountTierId,
            ListingId = listingId
        };

        return Result.Success(entity);
    }
}
