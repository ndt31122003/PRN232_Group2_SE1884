using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record ListingImportRow(
    int RowNumber,
    Guid ListingId,
    decimal? Price,
    int? Quantity,
    decimal? StartPrice,
    decimal? BuyItNowPrice,
    decimal? ReservePrice
);

public sealed record ImportListingsCsvCommand(
    IReadOnlyList<ListingImportRow> Rows
) : ICommand<ImportListingsCsvResult>;

public sealed record ImportListingsCsvResult(
    int UpdatedCount,
    IReadOnlyList<ImportFailure> Failures
);

public sealed record ImportFailure(
    int RowNumber,
    Guid? ListingId,
    string Message
);

public sealed class ImportListingsCsvCommandHandler : ICommandHandler<ImportListingsCsvCommand, ImportListingsCsvResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public ImportListingsCsvCommandHandler(IListingRepository listingRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ImportListingsCsvResult>> Handle(ImportListingsCsvCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.Rows is null || request.Rows.Count == 0)
        {
            return Error.Failure("Listing.EmptyImport", "Không có dữ liệu nào để cập nhật.");
        }

        var failures = new List<ImportFailure>();
        var updatedListings = new HashSet<Guid>();
        var cache = new Dictionary<Guid, Listing>();

        foreach (var row in request.Rows)
        {
            if (row.ListingId == Guid.Empty)
            {
                failures.Add(new ImportFailure(row.RowNumber, null, "ListingId không hợp lệ."));
                continue;
            }

            if (!cache.TryGetValue(row.ListingId, out var listing))
            {
                listing = await _listingRepository.GetByIdAsync(row.ListingId, cancellationToken);
                if (listing is null)
                {
                    failures.Add(new ImportFailure(row.RowNumber, row.ListingId, ListingErrors.NotFound.Description));
                    continue;
                }

                if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
                {
                    failures.Add(new ImportFailure(row.RowNumber, row.ListingId, ListingErrors.Unauthorized.Description));
                    continue;
                }

                cache[row.ListingId] = listing;
            }
            else if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
            {
                failures.Add(new ImportFailure(row.RowNumber, row.ListingId, ListingErrors.Unauthorized.Description));
                continue;
            }

            Result updateResult = listing switch
            {
                FixedPriceListing fixedPrice => UpdateFixedPriceListing(fixedPrice, row),
                AuctionListing auction => UpdateAuctionListing(auction, row),
                _ => Error.Failure("Listing.UnsupportedFormat", "Định dạng listing không được hỗ trợ cho cập nhật hàng loạt.")
            };

            if (updateResult.IsFailure)
            {
                failures.Add(new ImportFailure(row.RowNumber, row.ListingId, updateResult.Error.Description));
                continue;
            }

            updatedListings.Add(listing.Id);
        }

        if (updatedListings.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var result = new ImportListingsCsvResult(updatedListings.Count, failures);
        return Result.Success(result);
    }

    private static Result UpdateFixedPriceListing(FixedPriceListing listing, ListingImportRow row)
    {
        if (listing.Type == ListingType.MultiVariation)
        {
            return Error.Failure("Listing.MultiVariationNotSupported", "Không hỗ trợ cập nhật hàng loạt cho listing nhiều biến thể.");
        }

        if (listing.Status is not (ListingStatus.Draft or ListingStatus.Scheduled or ListingStatus.Active))
        {
            return Error.Failure("Listing.StatusNotEditable", "Không thể cập nhật listing ở trạng thái hiện tại.");
        }

        var newPrice = row.Price ?? listing.Pricing?.Price ?? 0m;
        var newQuantity = row.Quantity ?? listing.Pricing?.Quantity ?? 0;

        if (newPrice <= 0)
        {
            return Error.Failure("Listing.InvalidPrice", "Giá phải lớn hơn 0.");
        }

        if (newQuantity <= 0)
        {
            return Error.Failure("Listing.InvalidQuantity", "Số lượng phải lớn hơn 0.");
        }

        return listing.UpdatePricing(newPrice, newQuantity);
    }

    private static Result UpdateAuctionListing(AuctionListing listing, ListingImportRow row)
    {
        if (listing.Status == ListingStatus.Active)
        {
            return Error.Failure("Listing.AuctionActive", "Không thể cập nhật giá cho phiên đấu giá đang chạy.");
        }

        var newStartPrice = row.StartPrice ?? listing.Pricing.StartPrice;
        var newReservePrice = row.ReservePrice ?? listing.Pricing.ReservePrice;
        var newBuyItNowPrice = row.BuyItNowPrice ?? listing.Pricing.BuyItNowPrice;

        if (newStartPrice <= 0)
        {
            return Error.Failure("Listing.InvalidStartPrice", "Start price phải lớn hơn 0.");
        }

        return listing.UpdatePricing(newStartPrice, newReservePrice, newBuyItNowPrice, listing.Duration);
    }
}
