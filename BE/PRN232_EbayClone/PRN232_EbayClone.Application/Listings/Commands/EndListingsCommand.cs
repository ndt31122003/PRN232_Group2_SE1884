using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Common;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record EndListingsCommand(List<Guid> ListingIds) : ICommand<EndListingsResult>;

public sealed record EndListingsResult(
    int EndedCount,
    IReadOnlyList<Guid> ListingIds,
    IReadOnlyList<ListingActionFailure> Failures
);

public sealed class EndListingsCommandHandler : ICommandHandler<EndListingsCommand, EndListingsResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public EndListingsCommandHandler(IListingRepository listingRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<EndListingsResult>> Handle(EndListingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần kết thúc không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (distinctIds.Length == 0)
        {
            return Error.Failure("Listing.InvalidSelection", "Không tìm thấy listing hợp lệ để kết thúc.");
        }

        var endedIds = new List<Guid>(distinctIds.Length);
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

            var endResult = listing.End();
            if (endResult.IsFailure)
            {
                failures.Add(ListingActionFailure.FromError(listingId, endResult.Error));
                continue;
            }

            _listingRepository.Update(listing);
            endedIds.Add(listing.Id);
        }

        if (endedIds.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var response = new EndListingsResult(endedIds.Count, endedIds, failures);
        return Result.Success(response);
    }
}
