using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Common;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public enum RelistMode
{
    SameFormat,
    FixedPrice
}

public sealed record RelistListingsCommand(
    List<Guid> ListingIds,
    RelistMode Mode = RelistMode.SameFormat
) : ICommand<RelistListingsResult>;

public sealed record RelistListingsResult(
    int RelistedCount,
    IReadOnlyList<Guid> ListingIds
);

public sealed class RelistListingsCommandHandler : ICommandHandler<RelistListingsCommand, RelistListingsResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RelistListingsCommandHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<RelistListingsResult>> Handle(RelistListingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần relist không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Distinct()
            .ToArray();

        if (distinctIds.Any(id => id == Guid.Empty))
        {
            return Error.Failure("Listing.InvalidId", "Tồn tại listing id không hợp lệ.");
        }

        var createdListings = new List<Listing>(distinctIds.Length);

        foreach (var listingId in distinctIds)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
            if (listing is null)
            {
                return ListingErrors.NotFound;
            }

            if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
            {
                return ListingErrors.Unauthorized;
            }

            if (listing.Status != ListingStatus.Ended)
            {
                return Error.Failure("Listing.InvalidStatus", "Chỉ có thể relist các listing đã kết thúc.");
            }

            Result<Listing> duplicateResult = request.Mode switch
            {
                RelistMode.FixedPrice => ListingCloneFactory.CloneAsFixedPrice(listing, ListingStatus.Draft),
                _ => ListingCloneFactory.Clone(listing, ListingStatus.Draft)
            };

            if (duplicateResult.IsFailure)
            {
                return duplicateResult.Error;
            }

            var duplicate = duplicateResult.Value;
            var activateResult = duplicate.Activate();
            if (activateResult.IsFailure)
            {
                return activateResult.Error;
            }

            createdListings.Add(duplicate);
        }

        foreach (var created in createdListings)
        {
            _listingRepository.Add(created);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new RelistListingsResult(
            createdListings.Count,
            createdListings.Select(l => l.Id).ToList());

        return Result.Success(result);
    }
}
