using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record ExportListingsQuery(
    string? StatusSlug,
    string? SearchTerm,
    IReadOnlyCollection<Guid>? ListingIds,
    int MaxRows = 2000
) : IQuery<IReadOnlyList<ListingExportRow>>;

public sealed record ListingExportRow(
    Guid ListingId,
    ListingStatus Status,
    ListingFormat Format,
    string Title,
    string Sku,
    int? Quantity,
    decimal? Price,
    decimal? StartPrice,
    decimal? BuyItNowPrice,
    decimal? ReservePrice,
    DateTime? ScheduledStartTime,
    bool IsMultiVariation,
    string? VariationSummary
);

public sealed class ExportListingsQueryHandler : IQueryHandler<ExportListingsQuery, IReadOnlyList<ListingExportRow>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public ExportListingsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyList<ListingExportRow>>> Handle(ExportListingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var maxRows = Math.Clamp(request.MaxRows, 1, 5000);

        IReadOnlyCollection<ListingStatus>? statuses = null;

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            statuses = MapStatuses(request.StatusSlug);
        }

        var rows = await _listingRepository.GetListingsForExportAsync(
            userId,
            request.ListingIds,
            statuses,
            request.SearchTerm,
            maxRows,
            cancellationToken);

        return Result.Success(rows);
    }

    private static IReadOnlyCollection<ListingStatus> MapStatuses(string? statusSlug)
    {
        if (string.IsNullOrWhiteSpace(statusSlug))
        {
            return new[]
            {
                ListingStatus.Active,
                ListingStatus.Draft,
                ListingStatus.Scheduled
            };
        }

        return statusSlug.ToLowerInvariant() switch
        {
            "active" => new[] { ListingStatus.Active },
            "drafts" => new[] { ListingStatus.Draft },
            "scheduled" => new[] { ListingStatus.Scheduled },
            "ended" => new[] { ListingStatus.Ended },
            "unsold" => new[] { ListingStatus.Ended },
            _ => new[]
            {
                ListingStatus.Active,
                ListingStatus.Draft,
                ListingStatus.Scheduled
            }
        };
    }
}
