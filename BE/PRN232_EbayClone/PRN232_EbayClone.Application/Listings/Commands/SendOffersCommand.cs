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

public sealed record SendOffersCommand(List<Guid> ListingIds) : ICommand<SendOffersResult>;

public sealed record SendOffersResult(
    int QueuedCount,
    IReadOnlyList<ListingActionFailure> Failures
);

public sealed class SendOffersCommandHandler : ICommandHandler<SendOffersCommand, SendOffersResult>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public SendOffersCommandHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<SendOffersResult>> Handle(SendOffersCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        if (request.ListingIds is null || request.ListingIds.Count == 0)
        {
            return Error.Failure("Listing.EmptySelection", "Danh sách listing cần gửi ưu đãi không được để trống.");
        }

        var distinctIds = request.ListingIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (distinctIds.Length == 0)
        {
            return Error.Failure("Listing.InvalidSelection", "Không tìm thấy listing hợp lệ để gửi ưu đãi.");
        }

        var failures = new List<ListingActionFailure>();
        var queuedCount = 0;

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
                failures.Add(ListingActionFailure.InvalidSelection(listingId, "Chỉ có thể gửi ưu đãi cho listing đang hoạt động."));
                continue;
            }

            if (listing is FixedPriceListing fixedPrice && fixedPrice.OfferSettings.AllowOffers is false)
            {
                failures.Add(ListingActionFailure.InvalidSelection(listingId, "Listing không bật chế độ nhận offer."));
                continue;
            }

            failures.Add(new ListingActionFailure(listing.Id, "Listing.SendOffers.NotConfigured", "Tính năng gửi offer chưa được cấu hình trong môi trường này."));
        }

        var response = new SendOffersResult(queuedCount, failures);
        return Result.Success(response);
    }
}
