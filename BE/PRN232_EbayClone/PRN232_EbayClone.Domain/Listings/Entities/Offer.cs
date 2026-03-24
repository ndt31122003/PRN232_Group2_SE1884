using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Listings.Entities;

// Navigation property added
public sealed class Offer : AggregateRoot<Guid>
{
    public Guid ListingId { get; private set; }
    public Listing Listing { get; private set; } = null!; // Navigation property
    public string BuyerId { get; private set; } // Matches Identity User ID format
    public decimal Amount { get; private set; }
    public OfferStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Offer(Guid id, Guid listingId, string buyerId, decimal amount) : base(id)
    {
        ListingId = listingId;
        BuyerId = buyerId;
        Amount = amount;
        Status = OfferStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    // EF Core constructor
    private Offer() : base(Guid.NewGuid()) { }

    public static Offer Create(Guid listingId, string buyerId, decimal amount)
    {
        return new Offer(Guid.NewGuid(), listingId, buyerId, amount);
    }

    public void Accept() => Status = OfferStatus.Accepted;
    public void Decline() => Status = OfferStatus.Declined;
    public void Expire() => Status = OfferStatus.Expired;
}
