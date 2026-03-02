using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SellingPreferences.Errors;

public static class SellerPreferenceErrors
{
    public static readonly Error NotFound = Error.Failure(
        "SellerPreference.NotFound",
        "Seller preferences were not found for this seller.");

    public static readonly Error InvalidUnpaidItemStrikeCount = Error.Validation(
        "SellerPreference.InvalidUnpaidItemStrikeCount",
        "Unpaid item strike count must be between 1 and 4.");

    public static readonly Error InvalidUnpaidItemStrikePeriod = Error.Validation(
        "SellerPreference.InvalidUnpaidItemStrikePeriod",
        "Unpaid item strike period must be between 1 and 12 months.");

    public static readonly Error InvalidMaxItemsInLastTenDays = Error.Validation(
        "SellerPreference.InvalidMaxItemsInLastTenDays",
        "Max items in the last 10 days must be between 1 and 25.");

    public static readonly Error InvalidFeedbackScoreThreshold = Error.Validation(
        "SellerPreference.InvalidFeedbackScoreThreshold",
        "Feedback score threshold must be between 0 and 1000.");

    public static readonly Error InvalidInvoiceFormat = Error.Validation(
        "SellerPreference.InvalidInvoiceFormat",
        "Invoice format is invalid.");

    public static readonly Error BlockedBuyerLimitExceeded = Error.Validation(
        "SellerPreference.BlockedBuyerLimitExceeded",
        "You can block up to 5000 buyers.");

    public static readonly Error ExemptBuyerLimitExceeded = Error.Validation(
        "SellerPreference.ExemptBuyerLimitExceeded",
        "You can exempt up to 1000 buyers.");

    public static readonly Error DuplicateBlockedBuyer = Error.Validation(
        "SellerPreference.DuplicateBlockedBuyer",
        "Duplicate blocked buyers are not allowed.");

    public static readonly Error DuplicateExemptBuyer = Error.Validation(
        "SellerPreference.DuplicateExemptBuyer",
        "Duplicate exempt buyers are not allowed.");

    public static readonly Error BlockedBuyerIdentifierRequired = Error.Validation(
        "SellerPreference.BlockedBuyerIdentifierRequired",
        "Blocked buyer identifier cannot be empty.");

    public static readonly Error ExemptBuyerIdentifierRequired = Error.Validation(
        "SellerPreference.ExemptBuyerIdentifierRequired",
        "Exempt buyer identifier cannot be empty.");

    public static readonly Error BlockedBuyerIdentifierTooLong = Error.Validation(
        "SellerPreference.BlockedBuyerIdentifierTooLong",
        "Blocked buyer identifier cannot exceed 200 characters.");

    public static readonly Error ExemptBuyerIdentifierTooLong = Error.Validation(
        "SellerPreference.ExemptBuyerIdentifierTooLong",
        "Exempt buyer identifier cannot exceed 200 characters.");
}
