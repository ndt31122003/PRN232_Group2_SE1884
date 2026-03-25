using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Application.Listings.Inventory.Services;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.UpdateLowStockAlert;

public sealed record UpdateLowStockAlertCommand(
    Guid ListingId,
    int? ThresholdQuantity,
    bool EmailNotificationsEnabled) : ICommand<InventoryDto>;

public sealed class UpdateLowStockAlertCommandValidator : AbstractValidator<UpdateLowStockAlertCommand>
{
    public UpdateLowStockAlertCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");

        RuleFor(x => x.ThresholdQuantity)
            .GreaterThan(0)
            .When(x => x.ThresholdQuantity.HasValue)
            .WithMessage("Threshold quantity must be greater than 0.");

        RuleFor(x => x)
            .Must(x => !x.EmailNotificationsEnabled || x.ThresholdQuantity.HasValue)
            .WithMessage("Threshold quantity is required when email alerts are enabled.");
    }
}

public sealed class UpdateLowStockAlertCommandHandler : ICommandHandler<UpdateLowStockAlertCommand, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IInventoryLowStockNotifier _inventoryLowStockNotifier;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLowStockAlertCommandHandler(
        IInventoryRepository inventoryRepository,
        IListingRepository listingRepository,
        IInventoryLowStockNotifier inventoryLowStockNotifier,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _listingRepository = listingRepository;
        _inventoryLowStockNotifier = inventoryLowStockNotifier;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InventoryDto>> Handle(UpdateLowStockAlertCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to update low-stock alerts.");
        }

        var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(request.ListingId), cancellationToken);
        if (inventory is null)
        {
            return Error.Failure("Inventory.NotFound", "The inventory was not found.");
        }

        if (inventory.SellerId.Value != Guid.Parse(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var updateResult = inventory.ConfigureLowStockAlert(request.ThresholdQuantity, request.EmailNotificationsEnabled);
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        _inventoryRepository.Update(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is not null && await _inventoryLowStockNotifier.NotifyIfNeededAsync(inventory, listing.Title, listing.Sku, cancellationToken))
        {
            _inventoryRepository.Update(inventory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return inventory.ToDto();
    }
}