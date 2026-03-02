using PRN232_EbayClone.Domain.SaleEvents.Enums;
using PRN232_EbayClone.Domain.SaleEvents.Errors;
using PRN232_EbayClone.Domain.SaleEvents.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.SaleEvents.Entities;

public sealed class SaleEvent : AggregateRoot<Guid>
{
    private const int MaxNameLength = 90;

    private readonly List<SaleEventDiscountTier> _discountTiers = [];
    private readonly List<SaleEventListing> _listings = [];

    public UserId SellerId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public SaleEventMode Mode { get; private set; }
    public SaleEventStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool OfferFreeShipping { get; private set; }
    public bool IncludeSkippedItems { get; private set; }
    public bool BlockPriceIncreaseRevisions { get; private set; }
    public decimal? HighlightPercentage { get; private set; }

    public IReadOnlyCollection<SaleEventDiscountTier> DiscountTiers => _discountTiers.AsReadOnly();
    public IReadOnlyCollection<SaleEventListing> Listings => _listings.AsReadOnly();

    private SaleEvent(Guid id) : base(id)
    {
    }

    public static Result<SaleEvent> Create(
        UserId sellerId,
        string name,
        string? description,
        SaleEventMode mode,
        DateTime startDate,
        DateTime endDate,
        bool offerFreeShipping,
        bool includeSkippedItems,
        bool blockPriceIncreaseRevisions,
        decimal? highlightPercentage,
        IEnumerable<SaleEventDiscountTierDefinition>? tierDefinitions)
    {
        var saleEvent = new SaleEvent(Guid.NewGuid())
        {
            SellerId = sellerId,
            Name = name?.Trim() ?? string.Empty,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Mode = mode,
            StartDate = startDate,
            EndDate = endDate,
            OfferFreeShipping = offerFreeShipping,
            IncludeSkippedItems = includeSkippedItems,
            BlockPriceIncreaseRevisions = blockPriceIncreaseRevisions,
            HighlightPercentage = highlightPercentage,
            Status = DetermineInitialStatus(startDate, endDate),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = sellerId.ToString()
        };

        var tiers = tierDefinitions?.ToList() ?? []; // clones enumer

        var configureResult = saleEvent.ApplyModeConfiguration(tiers);
        if (configureResult.IsFailure)
        {
            return configureResult.Error;
        }

        var validationResult = Validate(saleEvent);
        if (validationResult.IsFailure)
        {
            return validationResult.Error;
        }

        return Result.Success(saleEvent);
    }

    public Result Update(
        string name,
        string? description,
        SaleEventMode mode,
        DateTime startDate,
        DateTime endDate,
        bool offerFreeShipping,
        bool includeSkippedItems,
        bool blockPriceIncreaseRevisions,
        decimal? highlightPercentage,
        IEnumerable<SaleEventDiscountTierDefinition>? tierDefinitions,
        DateTime utcNow)
    {
        if (Status is SaleEventStatus.Cancelled or SaleEventStatus.Ended)
        {
            return SaleEventErrors.EditNotAllowed;
        }

        Name = name?.Trim() ?? string.Empty;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Mode = mode;
        StartDate = startDate;
        EndDate = endDate;
        OfferFreeShipping = offerFreeShipping;
        IncludeSkippedItems = includeSkippedItems;
        BlockPriceIncreaseRevisions = blockPriceIncreaseRevisions;
        HighlightPercentage = highlightPercentage;

        var tiers = tierDefinitions?.ToList() ?? [];
        var configureResult = ApplyModeConfiguration(tiers);
        if (configureResult.IsFailure)
        {
            return configureResult.Error;
        }

        var validationResult = Validate(this);
        if (validationResult.IsFailure)
        {
            return validationResult.Error;
        }

        Status = DetermineStatusAfterUpdate(Status, StartDate, EndDate, utcNow);
        UpdatedAt = utcNow;
        UpdatedBy = SellerId.ToString();

        return Result.Success();
    }

    private Result ApplyModeConfiguration(IReadOnlyList<SaleEventDiscountTierDefinition> tierDefinitions)
    {
        switch (Mode)
        {
            case SaleEventMode.DiscountAndSaleEvent:
                if (tierDefinitions.Count == 0)
                {
                    return SaleEventErrors.DiscountTiersRequired;
                }

                if (HighlightPercentage.HasValue)
                {
                    HighlightPercentage = null;
                }

                return ConfigureDiscountTiers(tierDefinitions);

            case SaleEventMode.SaleEventOnly:
                if (tierDefinitions.Count > 0)
                {
                    return SaleEventErrors.InvalidModeConfiguration;
                }

                if (!HighlightPercentage.HasValue)
                {
                    return SaleEventErrors.HighlightPercentageRequired;
                }

                if (HighlightPercentage <= 0 || HighlightPercentage > 90)
                {
                    return SaleEventErrors.InvalidHighlightPercentage;
                }

                _discountTiers.Clear();
                _listings.Clear();
                return Result.Success();

            default:
                return SaleEventErrors.InvalidModeConfiguration;
        }
    }

    private Result ConfigureDiscountTiers(IReadOnlyList<SaleEventDiscountTierDefinition> tierDefinitions)
    {
        if (tierDefinitions.Count > 10)
        {
            return SaleEventErrors.TooManyDiscountTiers;
        }

        _discountTiers.Clear();
        _listings.Clear();

        var priorities = new HashSet<int>();
        var assignedListings = new HashSet<Guid>();

        foreach (var definition in tierDefinitions.OrderBy(t => t.Priority))
        {
            if (!priorities.Add(definition.Priority))
            {
                return SaleEventErrors.DuplicateTierPriority;
            }

            if (definition.DiscountType == SaleEventDiscountType.Percent)
            {
                if (definition.DiscountValue <= 0 || definition.DiscountValue > 90)
                {
                    return SaleEventErrors.InvalidDiscountPercentage;
                }
            }
            else if (definition.DiscountValue <= 0)
            {
                return SaleEventErrors.InvalidDiscountValue;
            }

            var tierOrError = SaleEventDiscountTier.Create(
                Id,
                definition.DiscountType,
                definition.DiscountValue,
                definition.Priority,
                definition.Label);

            if (tierOrError.IsFailure)
            {
                return tierOrError.Error;
            }

            var tier = tierOrError.Value;

            if (definition.ListingIds is null || definition.ListingIds.Count == 0)
            {
                return SaleEventErrors.ListingSelectionRequired;
            }

            foreach (var listingId in definition.ListingIds)
            {
                if (!assignedListings.Add(listingId))
                {
                    return SaleEventErrors.DuplicateListingAssignment;
                }

                var listingOrError = SaleEventListing.Create(Id, tier.Id, listingId);
                if (listingOrError.IsFailure)
                {
                    return listingOrError.Error;
                }

                var listing = listingOrError.Value;
                tier.AddListing(listing);
                _listings.Add(listing);
            }

            _discountTiers.Add(tier);
        }

        return Result.Success();
    }

    public Result UpdateStatus(SaleEventStatus newStatus, DateTime utcNow)
    {
        if (Status is SaleEventStatus.Ended or SaleEventStatus.Cancelled)
        {
            return SaleEventErrors.InvalidStatusTransition;
        }

        if (Status == newStatus)
        {
            return Result.Success();
        }

        if (!IsStatusTransitionAllowed(Status, newStatus, utcNow))
        {
            return SaleEventErrors.InvalidStatusTransition;
        }

        Status = newStatus;
        UpdatedAt = utcNow;
        UpdatedBy = SellerId.ToString();
        return Result.Success();
    }

    private static bool IsStatusTransitionAllowed(SaleEventStatus current, SaleEventStatus target, DateTime utcNow)
    {
        return current switch
        {
            SaleEventStatus.Draft => target is SaleEventStatus.Scheduled or SaleEventStatus.Active or SaleEventStatus.Cancelled,
            SaleEventStatus.Scheduled => target is SaleEventStatus.Draft or SaleEventStatus.Active or SaleEventStatus.Cancelled,
            SaleEventStatus.Active => target is SaleEventStatus.Ended or SaleEventStatus.Cancelled,
            _ => false
        };
    }

    private static Result Validate(SaleEvent saleEvent)
    {
        if (string.IsNullOrWhiteSpace(saleEvent.Name))
        {
            return SaleEventErrors.EmptyName;
        }

        if (saleEvent.Name.Length > MaxNameLength)
        {
            return SaleEventErrors.NameTooLong;
        }

        if (saleEvent.EndDate <= saleEvent.StartDate)
        {
            return SaleEventErrors.InvalidDateRange;
        }

        return Result.Success();
    }

    private static SaleEventStatus DetermineInitialStatus(DateTime startDate, DateTime endDate)
    {
        var now = DateTime.UtcNow;
        if (endDate <= now)
        {
            return SaleEventStatus.Ended;
        }

        if (startDate <= now && endDate > now)
        {
            return SaleEventStatus.Active;
        }

        if (startDate > now)
        {
            return SaleEventStatus.Scheduled;
        }

        return SaleEventStatus.Draft;
    }

    private static SaleEventStatus DetermineStatusAfterUpdate(
        SaleEventStatus current,
        DateTime startDate,
        DateTime endDate,
        DateTime utcNow)
    {
        if (current is SaleEventStatus.Cancelled or SaleEventStatus.Ended)
        {
            return current;
        }

        if (endDate <= utcNow)
        {
            return SaleEventStatus.Ended;
        }

        if (startDate <= utcNow && endDate > utcNow)
        {
            return SaleEventStatus.Active;
        }

        return startDate > utcNow ? SaleEventStatus.Scheduled : SaleEventStatus.Draft;
    }
}
