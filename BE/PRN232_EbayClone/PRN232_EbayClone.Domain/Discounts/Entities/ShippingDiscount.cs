using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Shipping discount - Giảm giá phí vận chuyển
/// </summary>
public sealed class ShippingDiscount : AggregateRoot<Guid>, IDiscount
{
    public DiscountType Type => DiscountType.ShippingDiscount;
    public UserId SellerId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal DiscountValue { get; private set; }
    public DiscountUnit DiscountUnit { get; private set; }
    public bool IsFreeShipping { get; private set; }
    public decimal? MinimumOrderValue { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }

    private ShippingDiscount(Guid id) : base(id) { }

    public static Result<ShippingDiscount> Create(
        UserId sellerId,
        string name,
        string? description,
        decimal discountValue,
        DiscountUnit discountUnit,
        bool isFreeShipping,
        decimal? minimumOrderValue,
        DateTime startDate,
        DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("ShippingDiscount.EmptyName", "Name cannot be empty");

        if (!isFreeShipping && discountValue <= 0)
            return Error.Validation("ShippingDiscount.InvalidValue", "Discount value must be greater than 0");

        if (discountUnit == DiscountUnit.Percent && discountValue > 100)
            return Error.Validation("ShippingDiscount.InvalidPercent", "Percent discount cannot exceed 100%");

        if (endDate <= startDate)
            return Error.Validation("ShippingDiscount.InvalidDateRange", "End date must be after start date");

        var discount = new ShippingDiscount(Guid.NewGuid())
        {
            SellerId = sellerId,
            Name = name.Trim(),
            Description = description?.Trim(),
            DiscountValue = discountValue,
            DiscountUnit = discountUnit,
            IsFreeShipping = isFreeShipping,
            MinimumOrderValue = minimumOrderValue,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(discount);
    }

    public Result<Money> CalculateDiscount(Money shippingCost, Money orderTotal)
    {
        if (!IsApplicable(DateTime.UtcNow))
            return Money.Zero(shippingCost.Currency);

        if (MinimumOrderValue.HasValue && orderTotal.Amount < MinimumOrderValue.Value)
            return Money.Zero(shippingCost.Currency);

        if (IsFreeShipping)
            return Money.Create(shippingCost.Amount, shippingCost.Currency);

        decimal discountAmount = DiscountUnit == DiscountUnit.Percent
            ? shippingCost.Amount * (DiscountValue / 100m)
            : DiscountValue;

        if (discountAmount > shippingCost.Amount)
            discountAmount = shippingCost.Amount;

        return Money.Create(discountAmount, shippingCost.Currency);
    }

    public Result<Money> CalculateDiscount(Money orderTotal, int itemCount)
    {
        return Money.Zero(orderTotal.Currency);
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
