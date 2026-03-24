using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Discounts.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Order discount - Giảm giá áp dụng trực tiếp cho đơn hàng
/// </summary>
public sealed class OrderDiscount : AggregateRoot<Guid>, IDiscount
{
    private readonly List<OrderDiscountTier> _tiers = [];
    private readonly List<OrderDiscountItemRule> _itemRules = [];
    private readonly List<OrderDiscountCategoryRule> _categoryRules = [];

    public DiscountType Type => DiscountType.OrderDiscount;
    public UserId SellerId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    // Threshold configuration
    public OrderDiscountThresholdType ThresholdType { get; private set; }
    public decimal? ThresholdAmount { get; private set; }
    public int? ThresholdQuantity { get; private set; }
    
    // Discount configuration
    public decimal DiscountValue { get; private set; }
    public DiscountUnit DiscountUnit { get; private set; }
    public decimal? MaxDiscount { get; private set; }
    
    // Eligibility rules
    public bool ApplyToAllItems { get; private set; }
    
    // Lifecycle
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    
    // Collections
    public IReadOnlyCollection<OrderDiscountTier> Tiers => _tiers.AsReadOnly();
    public IReadOnlyCollection<OrderDiscountItemRule> ItemRules => _itemRules.AsReadOnly();
    public IReadOnlyCollection<OrderDiscountCategoryRule> CategoryRules => _categoryRules.AsReadOnly();

    // Legacy property for backward compatibility
    public decimal? MinimumOrderValue => ThresholdType == OrderDiscountThresholdType.SpendBased ? ThresholdAmount : null;

    private OrderDiscount(Guid id) : base(id) { }

    public static Result<OrderDiscount> CreateSpendBased(
        UserId sellerId,
        string name,
        string? description,
        decimal discountValue,
        DiscountUnit discountUnit,
        decimal? maxDiscount,
        decimal thresholdAmount,
        DateTime startDate,
        DateTime endDate)
    {
        var validationResult = ValidateCommonFields(name, discountValue, discountUnit, startDate, endDate);
        if (validationResult.IsFailure)
            return validationResult;

        if (thresholdAmount <= 0)
            return Error.Validation("OrderDiscount.InvalidThresholdAmount", "Threshold amount must be greater than 0");

        var discount = new OrderDiscount(Guid.NewGuid())
        {
            SellerId = sellerId,
            Name = name.Trim(),
            Description = description?.Trim(),
            ThresholdType = OrderDiscountThresholdType.SpendBased,
            ThresholdAmount = thresholdAmount,
            ThresholdQuantity = null,
            DiscountValue = discountValue,
            DiscountUnit = discountUnit,
            MaxDiscount = maxDiscount,
            ApplyToAllItems = true,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(discount);
    }

    public static Result<OrderDiscount> CreateQuantityBased(
        UserId sellerId,
        string name,
        string? description,
        decimal discountValue,
        DiscountUnit discountUnit,
        decimal? maxDiscount,
        int thresholdQuantity,
        DateTime startDate,
        DateTime endDate)
    {
        var validationResult = ValidateCommonFields(name, discountValue, discountUnit, startDate, endDate);
        if (validationResult.IsFailure)
            return validationResult;

        if (thresholdQuantity <= 0)
            return Error.Validation("OrderDiscount.InvalidThresholdQuantity", "Threshold quantity must be a positive integer greater than 0");

        var discount = new OrderDiscount(Guid.NewGuid())
        {
            SellerId = sellerId,
            Name = name.Trim(),
            Description = description?.Trim(),
            ThresholdType = OrderDiscountThresholdType.QuantityBased,
            ThresholdAmount = null,
            ThresholdQuantity = thresholdQuantity,
            DiscountValue = discountValue,
            DiscountUnit = discountUnit,
            MaxDiscount = maxDiscount,
            ApplyToAllItems = true,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(discount);
    }

    private static Result<OrderDiscount> ValidateCommonFields(
        string name,
        decimal discountValue,
        DiscountUnit discountUnit,
        DateTime startDate,
        DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("OrderDiscount.EmptyName", "Name cannot be empty");

        if (discountValue <= 0)
            return Error.Validation("OrderDiscount.InvalidValue", "Discount value must be greater than 0");

        if (discountUnit == DiscountUnit.Percent && (discountValue < 0.01m || discountValue > 100))
            return Error.Validation("OrderDiscount.InvalidPercentValue", "Percent discount must be between 0.01 and 100");

        if (discountUnit == DiscountUnit.FixedAmount && discountValue <= 0)
            return Error.Validation("OrderDiscount.InvalidFixedValue", "Fixed amount discount must be greater than 0");

        if (endDate <= startDate)
            return Error.Validation("OrderDiscount.InvalidDateRange", "End date must be after start date");

        return Result.Success<OrderDiscount>(null!);
    }

    public Result AddTier(decimal thresholdValue, decimal discountValue)
    {
        if (_tiers.Count >= 10)
            return Error.Validation("OrderDiscountTier.TooManyTiers", "Cannot add more than 10 tiers");

        if (_tiers.Any(t => t.ThresholdValue == thresholdValue))
            return Error.Validation("OrderDiscountTier.DuplicateThreshold", "A tier with this threshold value already exists");

        var lastTier = _tiers.OrderByDescending(t => t.TierOrder).FirstOrDefault();
        if (lastTier != null)
        {
            if (thresholdValue <= lastTier.ThresholdValue)
                return Error.Validation("OrderDiscountTier.NonMonotonicThreshold", "Tier threshold must be higher than previous tier");

            if (discountValue < lastTier.DiscountValue)
                return Error.Validation("OrderDiscountTier.NonMonotonicDiscount", "Tier discount value must be greater than or equal to previous tier");
        }

        var tierOrder = _tiers.Count;
        var tierResult = OrderDiscountTier.Create(Id, thresholdValue, discountValue, tierOrder);
        
        if (tierResult.IsFailure)
            return tierResult.Error;

        _tiers.Add(tierResult.Value);
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result RemoveTier(Guid tierId)
    {
        var tier = _tiers.FirstOrDefault(t => t.Id == tierId);
        if (tier == null)
            return Error.Failure("OrderDiscountTier.NotFound", "Tier not found");

        _tiers.Remove(tier);
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result AddItemRule(Guid listingId, bool isExclusion)
    {
        if (_itemRules.Any(r => r.ListingId == listingId))
            return Error.Validation("OrderDiscount.DuplicateItemRule", "Item rule already exists");

        var ruleResult = OrderDiscountItemRule.Create(Id, listingId, isExclusion);
        if (ruleResult.IsFailure)
            return ruleResult.Error;

        _itemRules.Add(ruleResult.Value);
        ApplyToAllItems = false;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result AddCategoryRule(Guid categoryId, bool isExclusion)
    {
        if (_categoryRules.Any(r => r.CategoryId == categoryId))
            return Error.Validation("OrderDiscount.DuplicateCategoryRule", "Category rule already exists");

        var ruleResult = OrderDiscountCategoryRule.Create(Id, categoryId, isExclusion);
        if (ruleResult.IsFailure)
            return ruleResult.Error;

        _categoryRules.Add(ruleResult.Value);
        ApplyToAllItems = false;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result<bool> IsItemEligible(Guid listingId, Guid categoryId, DateTime lastPriceChange)
    {
        // Check 14-day price change waiting period
        if ((DateTime.UtcNow - lastPriceChange).TotalDays < 14)
            return Result.Success(false);

        if (ApplyToAllItems)
            return Result.Success(true);

        // Check exclusions first (exclusion takes precedence)
        var isExcludedByItem = _itemRules.Any(r => r.ListingId == listingId && r.IsExclusion);
        var isExcludedByCategory = _categoryRules.Any(r => r.CategoryId == categoryId && r.IsExclusion);
        
        if (isExcludedByItem || isExcludedByCategory)
            return Result.Success(false);

        // Check inclusions (union logic: item OR category)
        var isIncludedByItem = _itemRules.Any(r => r.ListingId == listingId && !r.IsExclusion);
        var isIncludedByCategory = _categoryRules.Any(r => r.CategoryId == categoryId && !r.IsExclusion);
        
        return Result.Success(isIncludedByItem || isIncludedByCategory);
    }

    public Result<DiscountCalculationResult> CalculateDiscount(
        IEnumerable<ValueObjects.OrderItem> items,
        DateTime currentDate)
    {
        if (!IsApplicable(currentDate))
        {
            var zeroMoney = Money.Zero("USD");
            if (zeroMoney.IsFailure)
                return zeroMoney.Error;

            return Result.Success(new DiscountCalculationResult(
                zeroMoney.Value,
                null,
                Array.Empty<Guid>(),
                Array.Empty<Guid>(),
                "Discount is not active or has expired"));
        }

        var eligibleItems = new List<Guid>();
        var excludedItems = new List<Guid>();
        var eligibleSubtotal = 0m;
        var eligibleQuantity = 0;
        var currency = items.FirstOrDefault()?.Price.Currency ?? "USD";

        foreach (var item in items)
        {
            var eligibilityResult = IsItemEligible(item.ListingId, item.CategoryId, item.LastPriceChange);
            if (eligibilityResult.IsFailure)
                continue;

            if (eligibilityResult.Value)
            {
                eligibleItems.Add(item.ListingId);
                eligibleSubtotal += item.Price.Amount * item.Quantity;
                eligibleQuantity += item.Quantity;
            }
            else
            {
                excludedItems.Add(item.ListingId);
            }
        }

        if (!eligibleItems.Any())
        {
            var zeroMoney = Money.Zero(currency);
            if (zeroMoney.IsFailure)
                return zeroMoney.Error;

            return Result.Success(new DiscountCalculationResult(
                zeroMoney.Value,
                null,
                eligibleItems,
                excludedItems,
                "No eligible items in order"));
        }

        // Check threshold
        bool meetsThreshold = ThresholdType == OrderDiscountThresholdType.SpendBased
            ? eligibleSubtotal >= (ThresholdAmount ?? 0)
            : eligibleQuantity >= (ThresholdQuantity ?? 0);

        if (!meetsThreshold)
        {
            var zeroMoney = Money.Zero(currency);
            if (zeroMoney.IsFailure)
                return zeroMoney.Error;

            return Result.Success(new DiscountCalculationResult(
                zeroMoney.Value,
                null,
                eligibleItems,
                excludedItems,
                "Order does not meet threshold requirements"));
        }

        // Determine applicable tier
        OrderDiscountTier? appliedTier = null;
        decimal effectiveDiscountValue = DiscountValue;

        if (_tiers.Any())
        {
            var thresholdValue = ThresholdType == OrderDiscountThresholdType.SpendBased
                ? eligibleSubtotal
                : eligibleQuantity;

            appliedTier = _tiers
                .Where(t => thresholdValue >= t.ThresholdValue)
                .OrderByDescending(t => t.ThresholdValue)
                .FirstOrDefault();

            if (appliedTier != null)
                effectiveDiscountValue = appliedTier.DiscountValue;
        }

        // Calculate discount amount
        decimal discountAmount = DiscountUnit == DiscountUnit.Percent
            ? eligibleSubtotal * (effectiveDiscountValue / 100m)
            : effectiveDiscountValue;

        // Apply caps
        if (MaxDiscount.HasValue && discountAmount > MaxDiscount.Value)
            discountAmount = MaxDiscount.Value;

        if (discountAmount > eligibleSubtotal)
            discountAmount = eligibleSubtotal;

        discountAmount = Math.Round(discountAmount, 2);

        var discountMoney = Money.Create(discountAmount, currency);
        if (discountMoney.IsFailure)
            return discountMoney.Error;

        return Result.Success(new DiscountCalculationResult(
            discountMoney.Value,
            appliedTier,
            eligibleItems,
            excludedItems,
            null));
    }

    // Legacy method for backward compatibility
    public Result<Money> CalculateDiscount(Money orderTotal, int itemCount)
    {
        if (!IsApplicable(DateTime.UtcNow))
        {
            var zeroResult = Money.Zero(orderTotal.Currency);
            return zeroResult;
        }

        if (MinimumOrderValue.HasValue && orderTotal.Amount < MinimumOrderValue.Value)
        {
            var zeroResult = Money.Zero(orderTotal.Currency);
            return zeroResult;
        }

        decimal discountAmount = DiscountUnit == DiscountUnit.Percent
            ? orderTotal.Amount * (DiscountValue / 100m)
            : DiscountValue;

        if (MaxDiscount.HasValue && discountAmount > MaxDiscount.Value)
            discountAmount = MaxDiscount.Value;

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
