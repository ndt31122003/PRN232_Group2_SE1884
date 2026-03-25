using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.SaleEvents.Services;

public interface ISaleEventEligibilityValidator
{
    Task<Result<bool>> ValidateListingEligibility(
        Guid listingId,
        Guid sellerId,
        CancellationToken cancellationToken = default);

    Result<bool> ValidateSaleEventEligibility(
        SaleEvent saleEvent,
        DateTime currentDate);

    Task<Result<bool>> ValidateListingForTierAssignment(
        Guid listingId,
        Guid saleEventId,
        Guid sellerId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<Guid>>> GetEligibleListings(
        Guid sellerId,
        string? searchTerm = null,
        Guid? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        CancellationToken cancellationToken = default);
}

public sealed class SaleEventEligibilityValidator : ISaleEventEligibilityValidator
{
    private readonly IListingRepository _listingRepository;

    public SaleEventEligibilityValidator(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Result<bool>> ValidateListingEligibility(
        Guid listingId,
        Guid sellerId,
        CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        
        if (listing == null)
        {
            return new Error(
                "SaleEventEligibilityValidator.ListingNotFound",
                "Listing not found");
        }

        // Check if listing is active and published
        if (listing.Status != ListingStatus.Active)
        {
            return new Error(
                "SaleEventEligibilityValidator.ListingNotActive",
                "Listing is not active");
        }

        // Check if listing is fixed price (not auction)
        if (listing.Format != ListingFormat.FixedPrice)
        {
            return new Error(
                "SaleEventEligibilityValidator.ListingNotFixedPrice",
                "Only fixed price listings are eligible for sale events");
        }

        return Result.Success(true);
    }

    public Result<bool> ValidateSaleEventEligibility(
        SaleEvent saleEvent,
        DateTime currentDate)
    {
        // Check if sale event is active
        if (saleEvent.Status != SaleEventStatus.Active)
        {
            return new Error(
                "SaleEventEligibilityValidator.SaleEventNotActive",
                "Sale event is not active");
        }

        // Check if current date is within sale event date range
        if (currentDate < saleEvent.StartDate)
        {
            return new Error(
                "SaleEventEligibilityValidator.SaleEventNotStarted",
                "Sale event has not started yet");
        }

        if (currentDate >= saleEvent.EndDate)
        {
            return new Error(
                "SaleEventEligibilityValidator.SaleEventExpired",
                "Sale event has expired");
        }

        return Result.Success(true);
    }

    public async Task<Result<bool>> ValidateListingForTierAssignment(
        Guid listingId,
        Guid saleEventId,
        Guid sellerId,
        CancellationToken cancellationToken = default)
    {
        // First validate basic listing eligibility
        var eligibilityResult = await ValidateListingEligibility(listingId, sellerId, cancellationToken);
        
        if (eligibilityResult.IsFailure)
        {
            return eligibilityResult;
        }

        return Result.Success(true);
    }

    public async Task<Result<IReadOnlyList<Guid>>> GetEligibleListings(
        Guid sellerId,
        string? searchTerm = null,
        Guid? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        CancellationToken cancellationToken = default)
    {
        // Use the existing repository method for getting eligible listings
        var (items, _) = await _listingRepository.GetEligibleListingsForSaleEventAsync(
            sellerId.ToString(),
            searchTerm,
            categoryId,
            minPrice,
            maxPrice,
            null, // minDaysOnSite
            false, // excludeAlreadyAssigned
            1, // pageNumber
            1000, // pageSize - get up to 1000 listings
            cancellationToken);

        var eligibleListingIds = items.Select(l => l.ListingId).ToList();

        return Result.Success<IReadOnlyList<Guid>>(eligibleListingIds);
    }
}
