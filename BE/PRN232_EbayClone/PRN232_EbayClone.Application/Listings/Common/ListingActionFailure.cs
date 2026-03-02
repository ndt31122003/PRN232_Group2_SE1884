using System;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Common;

public sealed record ListingActionFailure(Guid ListingId, string Code, string Message)
{
    public static ListingActionFailure FromError(Guid listingId, Error error)
    {
        var code = string.IsNullOrWhiteSpace(error.Code) ? "Listing.Action" : error.Code;
        var message = string.IsNullOrWhiteSpace(error.Description)
            ? "The operation could not be completed."
            : error.Description;

        return new ListingActionFailure(listingId, code, message);
    }

    public static ListingActionFailure NotFound(Guid listingId) =>
        new(listingId, "Listing.NotFound", "Listing was not found.");

    public static ListingActionFailure InvalidSelection(Guid listingId, string message) =>
        new(listingId, "Listing.InvalidSelection", message);
}
