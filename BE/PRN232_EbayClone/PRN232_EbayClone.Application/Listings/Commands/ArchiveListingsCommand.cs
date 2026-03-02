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

public sealed record ArchiveListingsCommand(List<Guid> ListingIds) : ICommand<ArchiveListingsResult>;

public sealed record ArchiveListingsResult(
    int ArchivedCount,
    IReadOnlyList<Guid> ListingIds,
    IReadOnlyList<ListingActionFailure> Failures
);

public sealed class ArchiveListingsCommandHandler : ICommandHandler<ArchiveListingsCommand, ArchiveListingsResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public ArchiveListingsCommandHandler(IListingRepository listingRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ArchiveListingsResult>> Handle(ArchiveListingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần lưu trữ không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (distinctIds.Length == 0)
        {
            return Error.Failure("Listing.InvalidSelection", "Không tìm thấy listing hợp lệ để lưu trữ.");
        }

        var archivedIds = new List<Guid>(distinctIds.Length);
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

            if (listing.Status != ListingStatus.Ended)
            {
                failures.Add(ListingActionFailure.InvalidSelection(listingId, "Chỉ có thể lưu trữ listing đã kết thúc."));
                continue;
            }

            if (listing.IsDeleted)
            {
                failures.Add(ListingActionFailure.InvalidSelection(listingId, "Listing đã ở trạng thái lưu trữ."));
                continue;
            }

            listing.IsDeleted = true;
            _listingRepository.Update(listing);
            archivedIds.Add(listing.Id);
        }

        if (archivedIds.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var response = new ArchiveListingsResult(archivedIds.Count, archivedIds, failures);
        return Result.Success(response);
    }
}
