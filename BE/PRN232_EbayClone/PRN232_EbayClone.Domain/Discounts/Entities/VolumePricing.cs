using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Volume pricing - Giảm giá theo số lượng mua
/// </summary>
public sealed class VolumePricing : AggregateRoot<Guid>, IDiscount
{
    private readonly List<VolumePricingTier> _tiers = [];

    public DiscountType Type => DiscountType.VolumePricing;
    public UserId SellerId { get; private set; }
    public Guid? ListingId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<VolumePricingTier> Tiers => _tiers.AsReadOnly();

    private VolumePricing(Guid id) : base(id) { }

    public static Result<VolumePricing> Create(
        UserId sellerId,
        Guid? listingId,
        string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        IEnumerable<VolumePricingTierDefinition> tierDefinitions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("VolumePricing.EmptyName", "Name cannot be empty");

        if (endDate <= startDate)
            return Error.Validation("VolumePricing.InvalidDateRange", "End date must be after start date");

        var tiers = tierDefinitions?.ToList() ?? [];
        if (tiers.Count == 0)
            return Error.Validation("VolumePricing.NoTiers", "At least one tier is required");

        var pricing = new VolumePricing(Guid.NewGuid())
        {
            SellerId = sellerId,
            ListingId = listingId,
            Name = name.Trim(),
            Description = description?.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var configureResult = pricing.ConfigureTiers(tiers);
        if (configureResult.IsFailure)
            return configureResult.Error;

        return Result.Success(pricing);
    }

    private Result ConfigureTiers(List<VolumePricingTierDefinition> tierDefinitions)
    {
        _tiers.Clear();

        var sortedTiers = tierDefinitions.OrderBy(t => t.MinQuantity).ToList();

        for (int i = 0; i < sortedTiers.Count; i++)
        {
            var def = sortedTiers[i];

            if (def.MinQuantity <= 0)
                return Error.Validation("VolumePricing.InvalidQuantity", "Minimum quantity must be greater than 0");

            if (def.DiscountValue <= 0)
                return Error.Validation("VolumePricing.InvalidValue", "Discount value must be greater than 0");

            if (def.DiscountUnit == DiscountUnit.Percent && def.DiscountValue > 100)
                return Error.Validation("VolumePricing.InvalidPercent", "Percent discount cannot exceed 100%");

            if (i > 0 && def.MinQuantity <= sortedTiers[i - 1].MinQuantity)
                return Error.Validation("VolumePricing.DuplicateQuantity", "Minimum quantities must be unique and increasing");

            var tier = new VolumePricingTier(
                Guid.NewGuid(),
                Id,
                def.MinQuantity,
                def.DiscountValue,
                def.DiscountUnit);

            _tiers.Add(tier);
        }

        return Result.Success();
    }

    public Result<Money> CalculateDiscount(Money orderTotal, int itemCount)
    {
        if (!IsApplicable(DateTime.UtcNow))
            return Money.Zero(orderTotal.Currency);

        var applicableTier = _tiers
            .Where(t => itemCount >= t.MinQuantity)
            .OrderByDescending(t => t.MinQuantity)
            .FirstOrDefault();

        if (applicableTier == null)
            return Money.Zero(orderTotal.Currency);

        decimal discountAmount = applicableTier.DiscountUnit == DiscountUnit.Percent
            ? orderTotal.Amount * (applicableTier.DiscountValue / 100m)
            : applicableTier.DiscountValue;

        return Money.Create(discountAmount, orderTotal.Currency);
    }

    public bool IsApplicable(DateTime currentDate)
    {
        return IsActive && currentDate >= StartDate && currentDate <= EndDate;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

public sealed class VolumePricingTier
{
    public Guid Id { get; private set; }
    public Guid VolumePricingId { get; private set; }
    public int MinQuantity { get; private set; }
    public decimal DiscountValue { get; private set; }
    public DiscountUnit DiscountUnit { get; private set; }

    public VolumePricingTier(Guid id, Guid volumePricingId, int minQuantity, decimal discountValue, DiscountUnit discountUnit)
    {
        Id = id;
        VolumePricingId = volumePricingId;
        MinQuantity = minQuantity;
        DiscountValue = discountValue;
        DiscountUnit = discountUnit;
    }
}

public sealed record VolumePricingTierDefinition(
    int MinQuantity,
    decimal DiscountValue,
    DiscountUnit DiscountUnit);
