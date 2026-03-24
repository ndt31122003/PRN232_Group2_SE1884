using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public sealed class Bid : AggregateRoot<Guid>
{
    public Guid ListingId { get; private set; }
    public Listing Listing { get; private set; } = null!; // Navigation property
    public string BidderId { get; private set; } // Matches Identity User ID format
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Bid(Guid id, Guid listingId, string bidderId, decimal amount) : base(id)
    {
        ListingId = listingId;
        BidderId = bidderId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }

    // EF Core constructor
    private Bid() : base(Guid.NewGuid()) { }

    public static Bid Create(Guid listingId, string bidderId, decimal amount)
    {
        return new Bid(Guid.NewGuid(), listingId, bidderId, amount);
    }
}
