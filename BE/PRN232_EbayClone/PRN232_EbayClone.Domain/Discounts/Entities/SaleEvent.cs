using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Discounts.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using OrderItemEntity = PRN232_EbayClone.Domain.Orders.Entities.OrderItem;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class SaleEvent : Entity<Guid>, IDiscount
{
    private readonly List<SaleEventDiscountTier> _discountTiers = new();
    private readonly List<SaleEventListing> _listings = new();

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid SellerId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public SaleEventMode Mode { get; private set; }
    public decimal? HighlightPercentage { get; private set; }
    public bool OfferFreeShipping { get; private set; }
    public bool BlockPriceIncreaseRevisions { get; private set; }
    public bool IncludeSkippedItems { get; private set; }
    public string? BuyerMessageLabel { get; private set; }
    public SaleEventStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SaleEventDiscountTier> DiscountTiers => _discountTiers.AsReadOnly();
    public IReadOnlyCollection<SaleEventListing> Listings => _listings.AsReadOnly();

    // IDiscount interface properties
    public DiscountType Type => DiscountType.SaleEvent;
    public bool IsActive => Status == SaleEventStatus.Active;

    private SaleEvent() : base(Guid.Empty) { }
    private SaleEvent(Guid id) : base(id) { }

    public static SaleEvent Create(
        string name,
        Guid sellerId,
        DateTime startDate,
        DateTime endDate,
        SaleEventMode mode,
        decimal? highlightPercentage = null,
        bool offerFreeShipping = false,
        bool blockPriceIncreaseRevisions = false,
        bool includeSkippedItems = false,
        string? description = null,
        string? buyerMessageLabel = null,
        List<SaleEventDiscountTierDefinition>? tiers = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Sale event name is required", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Sale event name cannot exceed 200 characters", nameof(name));

        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date", nameof(startDate));

        if (description?.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        if (buyerMessageLabel?.Length > 200)
            throw new ArgumentException("Buyer message label cannot exceed 200 characters", nameof(buyerMessageLabel));

        if (mode == SaleEventMode.SaleEventOnly && highlightPercentage.HasValue)
        {
            if (highlightPercentage.Value < 0.01m || highlightPercentage.Value > 90m)
                throw new ArgumentException("Highlight percentage must be between 0.01 and 90", nameof(highlightPercentage));
        }

        var saleEvent = new SaleEvent
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim(),
            SellerId = sellerId,
            StartDate = startDate,
            EndDate = endDate,
            Mode = mode,
            HighlightPercentage = highlightPercentage,
            OfferFreeShipping = offerFreeShipping,
            BlockPriceIncreaseRevisions = blockPriceIncreaseRevisions,
            IncludeSkippedItems = includeSkippedItems,
            BuyerMessageLabel = buyerMessageLabel?.Trim(),
            Status = SaleEventStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        if (tiers != null && tiers.Any())
        {
            if (mode == SaleEventMode.DiscountAndSaleEvent && !tiers.Any())
                throw new ArgumentException("At least one discount tier is required for DiscountAndSaleEvent mode", nameof(tiers));

            if (tiers.Count > 10)
                throw new ArgumentException("Cannot have more than 10 discount tiers", nameof(tiers));

            var priorities = tiers.Select(t => t.Priority).ToList();
            if (priorities.Distinct().Count() != priorities.Count)
                throw new ArgumentException("Tier priorities must be unique", nameof(tiers));

            foreach (var tierDef in tiers)
            {
                var tier = SaleEventDiscountTier.Create(
                    saleEvent.Id,
                    tierDef.DiscountType,
                    tierDef.DiscountValue,
                    tierDef.Priority,
                    tierDef.Label);

                saleEvent._discountTiers.Add(tier);

                foreach (var listingId in tierDef.ListingIds)
                {
                    var listing = SaleEventListing.Create(saleEvent.Id, tier.Id, listingId);
                    saleEvent._listings.Add(listing);
                    tier.AddListing(listing);
                }
            }
        }

        return saleEvent;
    }

    public void Update(
        string? name = null,
        string? description = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? buyerMessageLabel = null,
        bool? offerFreeShipping = null,
        bool? blockPriceIncreaseRevisions = null,
        bool? includeSkippedItems = null)
    {
        if (Status == SaleEventStatus.Active)
            throw new InvalidOperationException("Cannot update an active sale event");

        if (Status == SaleEventStatus.Ended)
            throw new InvalidOperationException("Cannot update an ended sale event");

        if (Status == SaleEventStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a cancelled sale event");

        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Sale event name is required", nameof(name));

            if (name.Length > 200)
                throw new ArgumentException("Sale event name cannot exceed 200 characters", nameof(name));

            Name = name.Trim();
        }

        if (description != null)
        {
            if (description.Length > 1000)
                throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

            Description = description.Trim();
        }

        if (startDate.HasValue || endDate.HasValue)
        {
            var newStartDate = startDate ?? StartDate;
            var newEndDate = endDate ?? EndDate;

            if (newStartDate >= newEndDate)
                throw new ArgumentException("Start date must be before end date");

            StartDate = newStartDate;
            EndDate = newEndDate;
        }

        if (buyerMessageLabel != null)
        {
            if (buyerMessageLabel.Length > 200)
                throw new ArgumentException("Buyer message label cannot exceed 200 characters", nameof(buyerMessageLabel));

            BuyerMessageLabel = buyerMessageLabel.Trim();
        }

        if (offerFreeShipping.HasValue)
            OfferFreeShipping = offerFreeShipping.Value;

        if (blockPriceIncreaseRevisions.HasValue)
            BlockPriceIncreaseRevisions = blockPriceIncreaseRevisions.Value;

        if (includeSkippedItems.HasValue)
            IncludeSkippedItems = includeSkippedItems.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == SaleEventStatus.Active)
            throw new InvalidOperationException("Sale event is already active");

        if (Status == SaleEventStatus.Ended)
            throw new InvalidOperationException("Cannot activate an ended sale event");

        if (Status == SaleEventStatus.Cancelled)
            throw new InvalidOperationException("Cannot activate a cancelled sale event");

        if (!_listings.Any())
            throw new InvalidOperationException("Cannot activate sale event without assigned listings");

        if (Mode == SaleEventMode.DiscountAndSaleEvent && !_discountTiers.Any())
            throw new InvalidOperationException("Cannot activate DiscountAndSaleEvent mode without discount tiers");

        var now = DateTime.UtcNow;
        if (now < StartDate)
            Status = SaleEventStatus.Scheduled;
        else if (now >= StartDate && now < EndDate)
            Status = SaleEventStatus.Active;
        else
            throw new InvalidOperationException("Cannot activate a sale event that has already ended");

        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status != SaleEventStatus.Active && Status != SaleEventStatus.Scheduled)
            throw new InvalidOperationException("Can only deactivate active or scheduled sale events");

        Status = SaleEventStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus()
    {
        var now = DateTime.UtcNow;

        if (Status == SaleEventStatus.Scheduled && now >= StartDate && now < EndDate)
        {
            Status = SaleEventStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
        else if ((Status == SaleEventStatus.Active || Status == SaleEventStatus.Scheduled) && now >= EndDate)
        {
            Status = SaleEventStatus.Ended;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddTier(SaleEventDiscountTier tier)
    {
        if (Status == SaleEventStatus.Active)
            throw new InvalidOperationException("Cannot add tiers to an active sale event");

        if (_discountTiers.Count >= 10)
            throw new InvalidOperationException("Cannot have more than 10 discount tiers");

        if (_discountTiers.Any(t => t.Priority == tier.Priority))
            throw new InvalidOperationException($"A tier with priority {tier.Priority} already exists");

        _discountTiers.Add(tier);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveTier(Guid tierId)
    {
        if (Status == SaleEventStatus.Active)
            throw new InvalidOperationException("Cannot remove tiers from an active sale event");

        var tier = _discountTiers.FirstOrDefault(t => t.Id == tierId);
        if (tier == null)
            throw new InvalidOperationException("Tier not found");

        var tierListings = _listings.Where(l => l.DiscountTierId == tierId).ToList();
        foreach (var listing in tierListings)
        {
            _listings.Remove(listing);
        }

        _discountTiers.Remove(tier);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTierPriority(Guid tierId, int newPriority)
    {
        if (Status == SaleEventStatus.Active)
            throw new InvalidOperationException("Cannot update tier priorities in an active sale event");

        var tier = _discountTiers.FirstOrDefault(t => t.Id == tierId);
        if (tier == null)
            throw new InvalidOperationException("Tier not found");

        if (_discountTiers.Any(t => t.Id != tierId && t.Priority == newPriority))
            throw new InvalidOperationException($"A tier with priority {newPriority} already exists");

        tier.UpdatePriority(newPriority);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignListingToTier(Guid listingId, Guid tierId)
    {
        var tier = _discountTiers.FirstOrDefault(t => t.Id == tierId);
        if (tier == null)
            throw new InvalidOperationException("Tier not found");

        if (_listings.Any(l => l.ListingId == listingId))
            throw new InvalidOperationException("Listing is already assigned to this sale event");

        var listing = SaleEventListing.Create(Id, tierId, listingId);
        _listings.Add(listing);
        tier.AddListing(listing);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveListingAssignment(Guid listingId)
    {
        var listing = _listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing == null)
            throw new InvalidOperationException("Listing assignment not found");

        var tier = _discountTiers.FirstOrDefault(t => t.Id == listing.DiscountTierId);
        tier?.RemoveListing(listingId);

        _listings.Remove(listing);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReassignListing(Guid listingId, Guid newTierId)
    {
        var listing = _listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing == null)
            throw new InvalidOperationException("Listing assignment not found");

        var newTier = _discountTiers.FirstOrDefault(t => t.Id == newTierId);
        if (newTier == null)
            throw new InvalidOperationException("Target tier not found");

        var oldTier = _discountTiers.FirstOrDefault(t => t.Id == listing.DiscountTierId);
        oldTier?.RemoveListing(listingId);

        listing.ReassignToTier(newTierId);
        newTier.AddListing(listing);
        UpdatedAt = DateTime.UtcNow;
    }

    public void BulkAssignListings(List<(Guid ListingId, Guid TierId)> assignments)
    {
        if (assignments.Count > 1000)
            throw new ArgumentException("Cannot assign more than 1000 listings at once", nameof(assignments));

        foreach (var (listingId, tierId) in assignments)
        {
            if (!_listings.Any(l => l.ListingId == listingId))
            {
                AssignListingToTier(listingId, tierId);
            }
        }
    }

    public SalePriceCalculationResult CalculateSalePrice(Guid listingId, decimal originalPrice)
    {
        if (Status != SaleEventStatus.Active)
            return new SalePriceCalculationResult(originalPrice, originalPrice, 0, null, "Sale event is not active");

        var listing = _listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing == null)
            return new SalePriceCalculationResult(originalPrice, originalPrice, 0, null, "Listing is not assigned to this sale event");

        if (Mode == SaleEventMode.SaleEventOnly)
        {
            return new SalePriceCalculationResult(originalPrice, originalPrice, 0, null, null);
        }

        var tier = _discountTiers.FirstOrDefault(t => t.Id == listing.DiscountTierId);
        if (tier == null)
            return new SalePriceCalculationResult(originalPrice, originalPrice, 0, null, "Discount tier not found");

        var discountAmount = tier.CalculateDiscountAmount(originalPrice);
        var salePrice = Math.Max(0, originalPrice - discountAmount);

        return new SalePriceCalculationResult(salePrice, originalPrice, discountAmount, tier.Label, null);
    }

    public bool IsListingEligible(Guid listingId)
    {
        if (Status != SaleEventStatus.Active)
            return false;

        var now = DateTime.UtcNow;
        if (now < StartDate || now >= EndDate)
            return false;

        return _listings.Any(l => l.ListingId == listingId);
    }

    public Result<Money> CalculateDiscount(Money orderTotal, int itemCount)
    {
        if (Status != SaleEventStatus.Active)
        {
            var zeroMoney = Money.Zero(orderTotal.Currency);
            return zeroMoney.IsFailure ? zeroMoney.Error : zeroMoney.Value;
        }

        if (Mode == SaleEventMode.SaleEventOnly)
        {
            var zeroMoney = Money.Zero(orderTotal.Currency);
            return zeroMoney.IsFailure ? zeroMoney.Error : zeroMoney.Value;
        }

        // For sale events, discount is calculated per-item based on tier assignments
        // This method returns zero as sale events apply discounts at the order level, not order level
        var zero = Money.Zero(orderTotal.Currency);
        return zero.IsFailure ? zero.Error : zero.Value;
    }

    public bool IsApplicable(DateTime currentDate)
    {
        return Status == SaleEventStatus.Active 
            && currentDate >= StartDate 
            && currentDate < EndDate;
    }

    public decimal CalculateDiscountForItems(List<OrderItemEntity> items)
    {
        if (Status != SaleEventStatus.Active)
            return 0;

        if (Mode == SaleEventMode.SaleEventOnly)
            return 0;

        decimal totalDiscount = 0;

        foreach (var item in items)
        {
            var listing = _listings.FirstOrDefault(l => l.ListingId == item.ListingId);
            if (listing != null)
            {
                var tier = _discountTiers.FirstOrDefault(t => t.Id == listing.DiscountTierId);
                if (tier != null)
                {
                    var discountAmount = tier.CalculateDiscountAmount(item.UnitPrice.Amount);
                    totalDiscount += discountAmount * item.Quantity;
                }
            }
        }

        return totalDiscount;
    }
}
