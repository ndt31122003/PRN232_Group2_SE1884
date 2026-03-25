using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Errors;

public static class InventoryErrors
{
    public static Error OutOfStock => Error.Failure(
        "Inventory.OutOfStock",
        "The requested quantity is not available in stock.");
    
    public static Error InvalidQuantity => Error.Failure(
        "Inventory.InvalidQuantity",
        "The quantity must be greater than zero.");
    
    public static Error InsufficientAvailableStock => Error.Failure(
        "Inventory.InsufficientAvailableStock",
        "There are not enough available items to perform this operation.");
    
    public static Error InvalidThreshold => Error.Failure(
        "Inventory.InvalidThreshold",
        "The threshold must be greater than zero.");

    public static readonly Error EmailAlertRequiresThreshold = Error.Failure(
        "Inventory.EmailAlertRequiresThreshold",
        "A low-stock threshold is required when email alerts are enabled.");

    public static readonly Error InvalidAdditionalEmail = Error.Failure(
        "Inventory.InvalidAdditionalEmail",
        "One or more additional alert email addresses are invalid.");
    
    public static Error NotFound => Error.Failure(
        "Inventory.NotFound",
        "The inventory was not found.");
    
    public static Error ReservationNotFound => Error.Failure(
        "Inventory.ReservationNotFound",
        "The reservation was not found.");
    
    public static Error ReservationExpired => Error.Failure(
        "Inventory.ReservationExpired",
        "The reservation has expired.");
    
    public static Error InvalidStateTransition => Error.Failure(
        "Inventory.InvalidStateTransition",
        "The operation is not valid in the current reservation state.");
}
