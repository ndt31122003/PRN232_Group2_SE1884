using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SaleEvents.Errors;

public static class SaleEventErrors
{
    public static readonly Error EmptyName = Error.Validation(
        "SaleEvent.EmptyName",
        "Sale event name is required.");

    public static readonly Error NameTooLong = Error.Validation(
        "SaleEvent.NameTooLong",
        "Sale event name cannot exceed 90 characters.");

    public static readonly Error InvalidDateRange = Error.Validation(
        "SaleEvent.InvalidDateRange",
        "End date must be after the start date.");

    public static readonly Error InvalidModeConfiguration = Error.Validation(
        "SaleEvent.InvalidModeConfiguration",
        "Sale event configuration does not match the selected mode.");

    public static readonly Error DiscountTiersRequired = Error.Validation(
        "SaleEvent.DiscountTiersRequired",
        "At least one discount tier is required.");

    public static readonly Error TooManyDiscountTiers = Error.Validation(
        "SaleEvent.TooManyDiscountTiers",
        "A sale event can include at most 10 discount tiers.");

    public static readonly Error InvalidDiscountValue = Error.Validation(
        "SaleEvent.InvalidDiscountValue",
        "Discount value must be greater than zero.");

    public static readonly Error InvalidDiscountPercentage = Error.Validation(
        "SaleEvent.InvalidDiscountPercentage",
        "Percent discounts must be between 1 and 90.");

    public static readonly Error DuplicateTierPriority = Error.Validation(
        "SaleEvent.DuplicateTierPriority",
        "Each discount tier must have a unique priority.");

    public static readonly Error DuplicateListingAssignment = Error.Validation(
        "SaleEvent.DuplicateListingAssignment",
        "Each listing can only be assigned to one discount tier per sale event.");

    public static readonly Error HighlightPercentageRequired = Error.Validation(
        "SaleEvent.HighlightPercentageRequired",
        "Highlight percentage is required in sale-event-only mode.");

    public static readonly Error InvalidHighlightPercentage = Error.Validation(
        "SaleEvent.InvalidHighlightPercentage",
        "Highlight percentage must be between 1 and 90.");

    public static readonly Error InvalidStatusTransition = Error.Validation(
        "SaleEvent.InvalidStatusTransition",
        "The requested status change is not allowed.");

    public static readonly Error EditNotAllowed = Error.Validation(
        "SaleEvent.EditNotAllowed",
        "This sale event can no longer be edited.");

    public static readonly Error Unauthorized = Error.Failure(
        "SaleEvent.Unauthorized",
        "User context is invalid.");

    public static readonly Error NameAlreadyExists = Error.Validation(
        "SaleEvent.NameAlreadyExists",
        "A sale event with the same name already exists.");

    public static readonly Error ListingSelectionRequired = Error.Validation(
        "SaleEvent.ListingSelectionRequired",
        "You must select at least one listing for each discount tier.");

    public static readonly Error ListingsNotOwnedBySeller = Error.Validation(
        "SaleEvent.ListingsNotOwnedBySeller",
        "One or more listings are not owned by the current seller.");

    public static readonly Error NotFound = Error.Failure(
        "SaleEvent.NotFound",
        "Sale event was not found.");

    public static Error PriceIncreaseBlocked(string saleEventName, decimal snapshotPrice) => Error.Validation(
        "SaleEvent.PriceIncreaseBlocked",
        $"Price increase is blocked by active sale event '{saleEventName}'. Original price was {snapshotPrice:C}. Price decreases are allowed.");
}
