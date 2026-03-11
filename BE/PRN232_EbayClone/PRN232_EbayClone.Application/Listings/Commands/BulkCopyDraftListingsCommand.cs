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

public sealed record BulkCopyDraftListingsCommand(
    List<Guid> ListingIds
) : ICommand;

public sealed class BulkCopyDraftListingsCommandHandler : ICommandHandler<BulkCopyDraftListingsCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public BulkCopyDraftListingsCommandHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(BulkCopyDraftListingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần sao chép không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Distinct()
            .ToArray();

        if (distinctIds.Any(id => id == Guid.Empty))
        {
            return Error.Failure("Listing.InvalidId", "Tồn tại listing id không hợp lệ.");
        }

        var duplicates = new List<Listing>(distinctIds.Length);

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

            if (listing.Status != ListingStatus.Draft)
            {
                return Error.Failure("Listing.InvalidStatus", "Chỉ có thể sao chép hàng loạt các listing ở trạng thái draft.");
            }

            var duplicateResult = ListingCloneFactory.Clone(listing, ListingStatus.Draft);
            if (duplicateResult.IsFailure)
            {
                return duplicateResult.Error;
            }

            var duplicate = duplicateResult.Value;
            var markDraftResult = duplicate.Draft();
            if (markDraftResult.IsFailure)
            {
                return markDraftResult.Error;
            }

            duplicates.Add(duplicate);
        }

        foreach (var duplicate in duplicates)
        {
            _listingRepository.Add(duplicate);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

