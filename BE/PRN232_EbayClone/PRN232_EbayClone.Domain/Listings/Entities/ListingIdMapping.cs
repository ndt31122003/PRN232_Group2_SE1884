using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public class ListingIdMapping : Entity<Guid>
{
    public Guid ListingId { get; private set; }
    public UserId SellerId { get; private set; }

    private ListingIdMapping(Guid listingId) : base(listingId) 
    {
        ListingId = listingId;
    }

    public static ListingIdMapping Create(Guid listingId, UserId sellerId)
    {
        var mapping = new ListingIdMapping(listingId)
        {
            SellerId = sellerId
        };

        return mapping;
    }
}