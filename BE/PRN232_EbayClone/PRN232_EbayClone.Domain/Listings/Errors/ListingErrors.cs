using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Errors;

public static class ListingErrors
{
    public static Error NotFound => Error.Failure(
        "Listing.NotFound",
        "The listing was not found.");

    public static Error Unauthorized => Error.Failure(
        "Unauthorized",
        "You are not authorized to access this listing.");
}
