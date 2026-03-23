using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class SaleEventDiscountTier : Entity<Guid>
{
    private readonly List<SaleEventListing> _listings = new();

    public Guid SaleEventId { get; private set; }
    public SaleEventDiscountType DiscountType { get; private set; }
    public decimal DiscountValue { get; private set; }
    public int Priority { get; private set; }
    public string? Label { get; private set; }

    public IReadOnlyCollection<SaleEventListing> Listings => _listings.AsReadOnly();

    private SaleEventDiscountTier() : base(Guid.Empty) { }
    private SaleEventDiscountTier(Guid id) : base(id) { }

    public static SaleEventDiscountTier Create(
        Guid saleEventId,
        SaleEventDiscountType discountType,
        decimal discountValue,
        int priority,
        string? label)
    {
        if (priority <= 0)
            throw new ArgumentException("Priority must be greater than 0", nameof(priority));

        if (discountValue <= 0)
            throw new ArgumentException("Discount value must be greater than 0", nameof(discountValue));

        if (discountType == SaleEventDiscountType.Percent && (discountValue < 0.01m || discountValue > 90m))
            throw new ArgumentException("Percentage discount must be between 0.01 and 90", nameof(discountValue));

        if (label != null && label.Length > 100)
            throw new ArgumentException("Label cannot exceed 100 characters", nameof(label));

        return new SaleEventDiscountTier
        {
            Id = Guid.NewGuid(),
            SaleEventId = saleEventId,
            DiscountType = discountType,
            DiscountValue = discountValue,
            Priority = priority,
            Label = label?.Trim()
        };
    }

    public void AddListing(SaleEventListing listing)
    {
        if (listing.DiscountTierId != Id)
            throw new InvalidOperationException("Listing does not belong to this tier");

        if (_listings.Any(l => l.ListingId == listing.ListingId))
            throw new InvalidOperationException("Listing is already assigned to this tier");

        _listings.Add(listing);
    }

    public void RemoveListing(Guid listingId)
    {
        var listing = _listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing != null)
        {
            _listings.Remove(listing);
        }
    }

    public void ClearListings()
    {
        _listings.Clear();
    }

    public void UpdatePriority(int newPriority)
    {
        if (newPriority <= 0)
            throw new ArgumentException("Priority must be greater than 0", nameof(newPriority));

        Priority = newPriority;
    }

    public decimal CalculateDiscountAmount(decimal originalPrice)
    {
        if (originalPrice <= 0)
            return 0;

        return DiscountType switch
        {
            SaleEventDiscountType.Percent => Math.Round(originalPrice * (DiscountValue / 100m), 2),
            SaleEventDiscountType.Amount => Math.Min(DiscountValue, originalPrice),
            _ => 0
        };
    }
}
