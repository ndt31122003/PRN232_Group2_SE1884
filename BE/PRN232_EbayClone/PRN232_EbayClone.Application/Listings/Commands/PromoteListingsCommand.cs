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

public sealed record PromoteListingsCommand(List<Guid> ListingIds) : ICommand<PromoteListingsResult>;

public sealed record PromoteListingsResult(
    int SubmittedCount,
    IReadOnlyList<ListingActionFailure> Failures
);

public sealed class PromoteListingsCommandHandler : ICommandHandler<PromoteListingsCommand, PromoteListingsResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public PromoteListingsCommandHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<PromoteListingsResult>> Handle(PromoteListingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần quảng bá không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (distinctIds.Length == 0)
        {
            return Error.Failure("Listing.InvalidSelection", "Không tìm thấy listing hợp lệ để quảng bá.");
        }

        var failures = new List<ListingActionFailure>();
        var submittedCount = 0;

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

            if (listing.Status != ListingStatus.Active)
            {
                failures.Add(ListingActionFailure.InvalidSelection(listingId, "Chỉ có thể quảng bá listing đang hoạt động."));
                continue;
            }

            failures.Add(new ListingActionFailure(listing.Id, "Listing.Promote.NotConfigured", "Tính năng quảng bá chưa được cấu hình trong môi trường này."));
        }

        var response = new PromoteListingsResult(submittedCount, failures);
        return Result.Success(response);
    }
}
