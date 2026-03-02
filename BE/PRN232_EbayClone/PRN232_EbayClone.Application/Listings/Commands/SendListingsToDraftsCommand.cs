using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Common;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record SendListingsToDraftsCommand(List<Guid> ListingIds) : ICommand<SendListingsToDraftsResult>;

public sealed record SendListingsToDraftsResult(
    int CreatedCount,
    IReadOnlyList<Guid> ListingIds,
    IReadOnlyList<ListingActionFailure> Failures
);

public sealed class SendListingsToDraftsCommandHandler : ICommandHandler<SendListingsToDraftsCommand, SendListingsToDraftsResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public SendListingsToDraftsCommandHandler(IListingRepository listingRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<SendListingsToDraftsResult>> Handle(SendListingsToDraftsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần chuyển về draft không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (distinctIds.Length == 0)
        {
            return Error.Failure("Listing.InvalidSelection", "Không tìm thấy listing hợp lệ để chuyển về draft.");
        }

        var createdListings = new List<Domain.Listings.Entities.Listing>(distinctIds.Length);
        var failures = new List<ListingActionFailure>();

        foreach (var listingId in distinctIds)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
            if (listing is null)
            {
                failures.Add(ListingActionFailure.NotFound(listingId));
                continue;
            }

            if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
            {
                failures.Add(ListingActionFailure.FromError(listingId, ListingErrors.Unauthorized));
                continue;
            }

            var cloneResult = ListingCloneFactory.Clone(listing, ListingStatus.Draft);
            if (cloneResult.IsFailure)
            {
                failures.Add(ListingActionFailure.FromError(listingId, cloneResult.Error));
                continue;
            }

            var clone = cloneResult.Value;
            var draftResult = clone.Draft();
            if (draftResult.IsFailure)
            {
                failures.Add(ListingActionFailure.FromError(listingId, draftResult.Error));
                continue;
            }

            createdListings.Add(clone);
        }

        foreach (var clone in createdListings)
        {
            _listingRepository.Add(clone);
        }

        if (createdListings.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var response = new SendListingsToDraftsResult(
            createdListings.Count,
            createdListings.Select(listing => listing.Id).ToList(),
            failures);

        return Result.Success(response);
    }
}
