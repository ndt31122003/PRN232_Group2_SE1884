using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Services;

public interface IPriceIncreaseValidator
{
    Task<Result<bool>> ValidatePriceChange(Guid listingId, decimal newPrice, CancellationToken cancellationToken = default);
}

public sealed class PriceIncreaseValidator : IPriceIncreaseValidator
{
    private readonly ISaleEventRepository _saleEventRepository;

    public PriceIncreaseValidator(ISaleEventRepository saleEventRepository)
    {
        _saleEventRepository = saleEventRepository;
    }

    public async Task<Result<bool>> ValidatePriceChange(Guid listingId, decimal newPrice, CancellationToken cancellationToken = default)
    {
        // Get all active sale events for this listing
        var activeSaleEvents = await _saleEventRepository.GetActiveSaleEventsForListingAsync(listingId, cancellationToken);

        // Check if any active sale event has price increase blocking enabled
        foreach (var saleEvent in activeSaleEvents)
        {
            if (!saleEvent.BlockPriceIncreaseRevisions)
            {
                continue;
            }

            // Get the price snapshot for this listing in this sale event
            var snapshot = await _saleEventRepository.GetPriceSnapshotAsync(saleEvent.Id, listingId, cancellationToken);
            
            if (snapshot == null)
            {
                // No snapshot found, allow the change
                continue;
            }

            // Check if the new price is higher than the snapshot price
            if (newPrice > snapshot.OriginalPrice)
            {
                return Result.Failure<bool>(SaleEventErrors.PriceIncreaseBlocked(saleEvent.Name, snapshot.OriginalPrice));
            }
        }

        return Result.Success(true);
    }
}
