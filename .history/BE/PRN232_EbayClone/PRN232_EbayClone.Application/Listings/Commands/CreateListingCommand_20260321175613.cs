using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record CreateListingCommand(
    ListingFormat Format,
    ListingType? Type, // chỉ dùng cho FixedPrice
    string Title,
    string Sku,
    string ListingDescription,
    Guid CategoryId,
    Guid? ConditionId,
    string ConditionDescription,
    IEnumerable<ItemSpecific> ItemSpecifics,
    IEnumerable<ListingImage>? ListingImages,
    decimal? Price,              // cho fixed price single
    int? Quantity,               // cho fixed price single
    IEnumerable<VariationDto>? Variations, // cho multi variation
    decimal? StartPrice,         // cho auction
    decimal? ReservePrice,
    decimal? BuyItNowPrice,
    Duration Duration,
    DateTime? ScheduledStartTime,
    bool AllowOffers,
    decimal? MinimumOffer,
    decimal? AutoAcceptOffer,
    bool IsDraft
) : ICommand;

public sealed class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");
        RuleFor(x => x.ListingDescription)
            .NotEmpty().WithMessage("Listing description is required.");
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
        RuleFor(x => x.ConditionDescription)
            .NotEmpty().WithMessage("Condition description is required.");
        RuleFor(x => x.Format)
            .IsInEnum().WithMessage("Invalid listing format.");
    }
}
public sealed class CreateListingCommandCommandHandler : ICommandHandler<CreateListingCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    public CreateListingCommandCommandHandler(
        IListingRepository listingRepository,
        IInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IUserContext userContext,
        IUserRepository userRepository)
    {
        _listingRepository = listingRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return CategoryErrors.NotFound;
        if (!category.IsLeaf)
            return CategoryErrors.NotLeaf;

        Result<Listing> createResult = request.Format switch
        {
            ListingFormat.FixedPrice when request.Type == ListingType.Single
                => HandleFixedPriceSingle(request, category.CategorySpecifics),

            ListingFormat.FixedPrice when request.Type == ListingType.MultiVariation
                => HandleFixedPriceMultiVariation(request, category.CategorySpecifics),

            ListingFormat.Auction
                => HandleAuction(request, category.CategorySpecifics),

            _ => Error.Failure("Listing.InvalidFormat", "Unsupported listing format.")
        };

        if (createResult.IsFailure) return createResult.Error;

        var createdListing = createResult.Value;

        if (!Guid.TryParse(userId, out var sellerGuid))
        {
            return ListingErrors.Unauthorized;
        }

        var seller = await _userRepository.GetByIdAsync(
            new UserId(sellerGuid),
            cancellationToken);
        if (seller is null)
            return UserErrors.NotFound;

        var addListingResult = seller.AddListing(createdListing);
        if (addListingResult.IsFailure)
            return addListingResult.Error;

        Result statusResult = request switch
        {
            { IsDraft: true } => createdListing.Draft(),
            { ScheduledStartTime: not null } => createdListing.Schedule(request.ScheduledStartTime.Value),
            _ => createdListing.Activate()
        };
        if (statusResult.IsFailure)
            return statusResult.Error;

        var inventoryResult = Domain.Listings.Inventory.Entities.Inventory.Create(
            new ListingId(createdListing.Id),
            seller.Id,
            ResolveInitialQuantity(createdListing));
        if (inventoryResult.IsFailure)
            return inventoryResult.Error;

        _listingRepository.Add(createdListing);
        _inventoryRepository.Add(inventoryResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result<Listing> HandleFixedPriceSingle(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics)
    {
        var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, categorySpecifics);
        if (validateResult.IsFailure) return validateResult.Error;

        var listingOrError = FixedPriceListing.CreateSingle(
            request.Title,
            request.Sku,
            request.ListingDescription,
            request.CategoryId,
            request.ConditionId,
            request.ConditionDescription,
            request.ItemSpecifics,
            request.Price!.Value,
            request.AllowOffers,
            request.MinimumOffer,
            request.AutoAcceptOffer,
            request.ListingImages ?? [],
            request.Quantity!.Value
        );
        if (listingOrError.IsFailure) return listingOrError.Error;
        return listingOrError.Value;
    }

    private static Result<Listing> HandleFixedPriceMultiVariation(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics)
    {
        var variations = new List<Variation>();
        foreach (var v in request.Variations ?? [])
        {
            var validateResult = Category.ValidateSpecifics(v.VariationSpecifics, categorySpecifics);
            if (validateResult.IsFailure) return validateResult.Error;

            var variationOrError = Variation.Create(
                v.Sku,
                v.Price,
                v.VariationSpecifics,
                v.VariationImages ?? [],
                v.Quantity
            );
            if (variationOrError.IsFailure) return variationOrError.Error;

            variations.Add(variationOrError.Value);
        }

        var listingOrError = FixedPriceListing.CreateWithMultiVariation(
            request.Title,
            request.Sku,
            request.ListingDescription,
            request.CategoryId,
            request.ConditionId,
            request.ConditionDescription,
            request.AllowOffers,
            request.MinimumOffer,
            request.AutoAcceptOffer,
            variations
        );
        if (listingOrError.IsFailure) return listingOrError.Error;
        return listingOrError.Value;
    }

    private static Result<Listing> HandleAuction(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics)
    {
        var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, categorySpecifics);
        if (validateResult.IsFailure) return validateResult.Error;

        var listingOrError = AuctionListing.Create(
            request.Title,
            request.Sku,
            request.ListingDescription,
            request.CategoryId,
            request.ConditionId,
            request.ConditionDescription,
            request.ItemSpecifics,
            request.StartPrice!.Value,
            request.ReservePrice,
            request.BuyItNowPrice,
            request.Duration,
            request.ListingImages ?? []
        );
        if (listingOrError.IsFailure) return listingOrError.Error;
        return listingOrError.Value;
    }

    private static int ResolveInitialQuantity(Listing listing)
    {
        return listing switch
        {
            FixedPriceListing fixedPriceListing when fixedPriceListing.Type == ListingType.Single
                => fixedPriceListing.Pricing.Quantity,
            FixedPriceListing fixedPriceListing when fixedPriceListing.Type == ListingType.MultiVariation
                => fixedPriceListing.Variations.Sum(x => x.Quantity),
            AuctionListing auctionListing
                => auctionListing.Pricing.Quantity,
            _ => 0
        };
    }

}