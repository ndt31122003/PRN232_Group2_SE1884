using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.RestockInventory;

public sealed record RestockInventoryCommand(
    Guid ListingId,
    int Quantity,
    string? Reason) : ICommand<InventoryDto>;

public sealed class RestockInventoryCommandValidator : AbstractValidator<RestockInventoryCommand>
{
    public RestockInventoryCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Restock quantity must be greater than 0.");

        RuleFor(x => x.Reason)
            .MaximumLength(250)
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage("Reason must be 250 characters or fewer.");
    }
}

public sealed class RestockInventoryCommandHandler : ICommandHandler<RestockInventoryCommand, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public RestockInventoryCommandHandler(
        IInventoryRepository inventoryRepository,
        IListingRepository listingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InventoryDto>> Handle(RestockInventoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to restock inventory.");
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
        {
            return ListingErrors.NotFound;
        }

        if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
        {
            return ListingErrors.Unauthorized;
        }

        var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(request.ListingId), cancellationToken);
        if (inventory is null)
        {
            return Error.Failure("Inventory.NotFound", "The inventory was not found.");
        }

        var result = inventory.Restock(request.Quantity, request.Reason);
        if (result.IsFailure)
        {
            return result.Error;
        }

        _inventoryRepository.Update(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return inventory.ToDto();
    }
}