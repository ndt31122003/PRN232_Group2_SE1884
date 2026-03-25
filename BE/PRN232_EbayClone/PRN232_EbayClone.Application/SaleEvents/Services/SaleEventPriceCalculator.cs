using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Discounts.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.SaleEvents.Services;

public interface ISaleEventPriceCalculator
{
    Result<SalePriceCalculationResult> CalculateSalePrice(
        SaleEvent saleEvent,
        Guid listingId,
        decimal originalPrice,
        DateTime currentDate);

    Result<decimal> CalculateDiscountAmount(
        SaleEventDiscountType discountType,
        decimal discountValue,
        decimal originalPrice);

    Result<IReadOnlyList<SalePriceCalculationResult>> CalculateBulkSalePrices(
        SaleEvent saleEvent,
        IEnumerable<(Guid ListingId, decimal OriginalPrice)> listings,
        DateTime currentDate);
}

public sealed class SaleEventPriceCalculator : ISaleEventPriceCalculator
{
    public Result<SalePriceCalculationResult> CalculateSalePrice(
        SaleEvent saleEvent,
        Guid listingId,
        decimal originalPrice,
        DateTime currentDate)
    {
        // Validate inputs
        if (originalPrice < 0)
        {
            return new Error(
                "SaleEventPriceCalculator.InvalidOriginalPrice",
                "Original price cannot be negative");
        }

        // Check if sale event is active
        if (saleEvent.Status != SaleEventStatus.Active)
        {
            return new SalePriceCalculationResult(
                originalPrice,
                originalPrice,
                0,
                null,
                "Sale event is not active");
        }

        // Check if current date is within sale event date range
        if (currentDate < saleEvent.StartDate || currentDate >= saleEvent.EndDate)
        {
            return new SalePriceCalculationResult(
                originalPrice,
                originalPrice,
                0,
                null,
                "Sale event is not within active date range");
        }

        // Check if listing is assigned to the sale event
        var listing = saleEvent.Listings.FirstOrDefault(l => l.ListingId == listingId);
        if (listing == null)
        {
            return new SalePriceCalculationResult(
                originalPrice,
                originalPrice,
                0,
                null,
                "Listing is not assigned to this sale event");
        }

        // For SaleEventOnly mode, return original price (no discount calculation)
        if (saleEvent.Mode == SaleEventMode.SaleEventOnly)
        {
            return new SalePriceCalculationResult(
                originalPrice,
                originalPrice,
                0,
                null,
                null);
        }

        // Find the tier for this listing
        var tier = saleEvent.DiscountTiers.FirstOrDefault(t => t.Id == listing.DiscountTierId);
        if (tier == null)
        {
            return new SalePriceCalculationResult(
                originalPrice,
                originalPrice,
                0,
                null,
                "Discount tier not found");
        }

        // Calculate discount amount
        var discountResult = CalculateDiscountAmount(tier.DiscountType, tier.DiscountValue, originalPrice);
        if (discountResult.IsFailure)
        {
            return discountResult.Error;
        }

        var discountAmount = discountResult.Value;

        // Calculate sale price and ensure non-negative
        var salePrice = originalPrice - discountAmount;
        salePrice = Math.Max(0, salePrice);

        // Round to 2 decimal places
        salePrice = Math.Round(salePrice, 2);
        discountAmount = Math.Round(discountAmount, 2);

        return new SalePriceCalculationResult(
            salePrice,
            originalPrice,
            discountAmount,
            tier.Label,
            null);
    }

    public Result<decimal> CalculateDiscountAmount(
        SaleEventDiscountType discountType,
        decimal discountValue,
        decimal originalPrice)
    {
        // Validate inputs
        if (originalPrice < 0)
        {
            return new Error(
                "SaleEventPriceCalculator.InvalidOriginalPrice",
                "Original price cannot be negative");
        }

        if (discountValue <= 0)
        {
            return new Error(
                "SaleEventPriceCalculator.InvalidDiscountValue",
                "Discount value must be greater than zero");
        }

        decimal discountAmount = discountType switch
        {
            SaleEventDiscountType.Percent => originalPrice * (discountValue / 100m),
            SaleEventDiscountType.Amount => Math.Min(discountValue, originalPrice),
            _ => 0
        };

        // Round to 2 decimal places
        discountAmount = Math.Round(discountAmount, 2);

        return Result.Success(discountAmount);
    }

    public Result<IReadOnlyList<SalePriceCalculationResult>> CalculateBulkSalePrices(
        SaleEvent saleEvent,
        IEnumerable<(Guid ListingId, decimal OriginalPrice)> listings,
        DateTime currentDate)
    {
        var results = new List<SalePriceCalculationResult>();

        foreach (var (listingId, originalPrice) in listings)
        {
            var result = CalculateSalePrice(saleEvent, listingId, originalPrice, currentDate);
            
            if (result.IsSuccess)
            {
                results.Add(result.Value);
            }
            else
            {
                // For bulk operations, include failed calculations with error message
                results.Add(new SalePriceCalculationResult(
                    originalPrice,
                    originalPrice,
                    0,
                    null,
                    result.Error.Description));
            }
        }

        return Result.Success<IReadOnlyList<SalePriceCalculationResult>>(results);
    }
}
