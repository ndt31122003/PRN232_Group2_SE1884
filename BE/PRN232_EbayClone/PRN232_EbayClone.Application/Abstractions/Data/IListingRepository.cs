using PRN232_EbayClone.Application.Listings.Dtos;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IListingRepository : IRepository<Listing, Guid>
{
    Task<List<Listing>> GetBySellerIdAsync(string sellerId, CancellationToken cancellationToken = default);
    Task<List<Listing>> GetListingsToActivateAsync(DateTime now, int batchSize = 100, CancellationToken cancellationToken = default);
    Task<List<Listing>> GetListingsToEndAsync(DateTime now, int batchSize = 100, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ActiveListingDto> Items, int TotalCount)> GetActiveListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<DraftListingDto> Items, int TotalCount)> GetDraftListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ScheduledListingDto> Items, int TotalCount)> GetScheduledListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<EndedListingDto> Items, int TotalCount)> GetEndedListingsAsync(
        string ownerId,
        string? searchTerm,
        SoldStatus? soldStatus,
        RelistStatus? relistStatus,
        DateTime? fromDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ListingExportRow>> GetListingsForExportAsync(
        string ownerId,
        IReadOnlyCollection<Guid>? listingIds,
        IReadOnlyCollection<ListingStatus>? statuses,
        string? searchTerm,
        int maxRows,
        CancellationToken cancellationToken = default);
    Task<ListingOverviewSnapshot> GetOverviewSnapshotAsync(string ownerId, DateTime todayUtc, CancellationToken cancellationToken = default);

    Task<ProductResearchActiveListingsPage> GetProductResearchActiveListingsAsync(
        string? searchTerm,
        int page,
        int pageSize,
        Guid? categoryId,
        ListingFormat? format,
        decimal? minPrice,
        decimal? maxPrice,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, ListingFormat>> GetListingFormatsAsync(
        IReadOnlyCollection<Guid> listingIds,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<SaleEventEligibleListingDto> Items, int TotalCount)> GetEligibleListingsForSaleEventAsync(
        string ownerId,
        string? searchTerm,
        Guid? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int? minDaysOnSite,
        bool excludeAlreadyAssigned,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
