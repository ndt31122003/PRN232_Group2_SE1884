using PRN232_EbayClone.Domain.SaleEvents.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SaleEvents.Entities;

public sealed class SaleEventDiscountTier : Entity<Guid>
{
    private readonly List<SaleEventListing> _listings = [];

    public Guid SaleEventId { get; private set; }
    public SaleEventDiscountType DiscountType { get; private set; }
    public decimal DiscountValue { get; private set; }
    public int Priority { get; private set; }
    public string? Label { get; private set; }

    public IReadOnlyCollection<SaleEventListing> Listings => _listings.AsReadOnly();

    private SaleEventDiscountTier(Guid id) : base(id)
    {
    }

    public static Result<SaleEventDiscountTier> Create(
        Guid saleEventId,
        SaleEventDiscountType discountType,
        decimal discountValue,
        int priority,
        string? label)
    {
        if (discountValue <= 0)
        {
            return Error.Validation(
                "SaleEventDiscountTier.InvalidDiscountValue",
                "Discount value must be greater than zero.");
        }

        if (priority <= 0)
        {
            return Error.Validation(
                "SaleEventDiscountTier.InvalidPriority",
                "Priority must be greater than zero.");
        }

        var tier = new SaleEventDiscountTier(Guid.NewGuid())
        {
            SaleEventId = saleEventId,
            DiscountType = discountType,
            DiscountValue = discountValue,
            Priority = priority,
            Label = string.IsNullOrWhiteSpace(label) ? null : label.Trim()
        };

        return Result.Success(tier);
    }

    public Result AddListing(SaleEventListing listing)
    {
        if (listing.DiscountTierId != Id)
        {
            return Error.Validation(
                "SaleEventDiscountTier.InvalidListing",
                "Listing tier mismatch.");
        }

        _listings.Add(listing);
        return Result.Success();
    }

    public void ClearListings() => _listings.Clear();
}
