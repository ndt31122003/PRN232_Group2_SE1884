using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.InitializeInventory;

public sealed record InitializeInventoryCommand(
    Guid ListingId,
    int? InitialQuantity = null,
    int? ThresholdQuantity = null) : ICommand<InventoryDto>;

public sealed class InitializeInventoryCommandValidator : AbstractValidator<InitializeInventoryCommand>
{
    public InitializeInventoryCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");

        RuleFor(x => x.InitialQuantity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.InitialQuantity.HasValue)
            .WithMessage("Initial quantity must be greater than or equal to 0.");

        RuleFor(x => x.ThresholdQuantity)
            .GreaterThan(0)
            .When(x => x.ThresholdQuantity.HasValue)
            .WithMessage("Threshold quantity must be greater than 0.");
    }
}

public sealed class InitializeInventoryCommandHandler : ICommandHandler<InitializeInventoryCommand, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public InitializeInventoryCommandHandler(
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

    public async Task<Result<InventoryDto>> Handle(InitializeInventoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to initialize inventory.");
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

        var listingId = new ListingId(request.ListingId);
        var alreadyExists = await _inventoryRepository.ExistsForListingAsync(listingId, cancellationToken);
        if (alreadyExists)
        {
            return Error.Failure("Inventory.AlreadyExists", "Inventory already exists for this listing.");
        }

        var sellerId = new UserId(Guid.Parse(userId));
        var initialQuantity = request.InitialQuantity ?? ResolveInitialQuantity(listing);

        var inventoryResult = Domain.Listings.Inventory.Entities.Inventory.Create(listingId, sellerId, initialQuantity);
        if (inventoryResult.IsFailure)
        {
            return inventoryResult.Error;
        }

        var inventory = inventoryResult.Value;

        if (request.ThresholdQuantity.HasValue)
        {
            var thresholdResult = inventory.SetLowStockThreshold(request.ThresholdQuantity.Value);
            if (thresholdResult.IsFailure)
            {
                return thresholdResult.Error;
            }
        }

        _inventoryRepository.Add(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return inventory.ToDto();
    }

    private static int ResolveInitialQuantity(Listing listing)
    {
        return listing switch
        {
            FixedPriceListing fixedPriceListing when fixedPriceListing.Type == Domain.Listings.Enums.ListingType.Single
                => fixedPriceListing.Pricing.Quantity,
            FixedPriceListing fixedPriceListing when fixedPriceListing.Type == Domain.Listings.Enums.ListingType.MultiVariation
                => fixedPriceListing.Variations.Sum(x => x.Quantity),
            AuctionListing auctionListing
                => auctionListing.Pricing.Quantity,
            _ => 0
        };
    }
}