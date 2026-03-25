using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Storage;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record CreateVariationDto(
    string Sku, 
    decimal Price, 
    int Quantity, 
    IEnumerable<VariationSpecific> VariationSpecifics, 
    IEnumerable<ListingImage>? VariationImages,
    IEnumerable<string>? Base64Images
);

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
    IEnumerable<string>? Base64Images,
    decimal? Price,              // cho fixed price single
    int? Quantity,               // cho fixed price single
    IEnumerable<CreateVariationDto>? Variations, // cho multi variation
    decimal? StartPrice,         // cho auction
    decimal? ReservePrice,
    decimal? BuyItNowPrice,
    Duration Duration,
    DateTime? ScheduledStartTime,
    bool AllowOffers,
    decimal? MinimumOffer,
    decimal? AutoAcceptOffer,
    Guid? ShippingPolicyId,
    Guid? ReturnPolicyId,
    bool IsDraft
) : ICommand;

public sealed class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(80).WithMessage("Title must not exceed 80 characters.");
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
    private readonly ICloudinaryService _cloudinaryService;

    public CreateListingCommandCommandHandler(
        IListingRepository listingRepository,
        IInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IUserContext userContext,
        IUserRepository userRepository,
        ICloudinaryService cloudinaryService)
    {
        _listingRepository = listingRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _userContext = userContext;
        _userRepository = userRepository;
        _cloudinaryService = cloudinaryService;
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
                => await HandleFixedPriceSingleAsync(request, category.CategorySpecifics, cancellationToken),

            ListingFormat.FixedPrice when request.Type == ListingType.MultiVariation
                => await HandleFixedPriceMultiVariationAsync(request, category.CategorySpecifics, cancellationToken),

            ListingFormat.Auction
                => await HandleAuctionAsync(request, category.CategorySpecifics, cancellationToken),

            _ => Error.Failure("Listing.InvalidFormat", "Unsupported listing format.")
        };

        if (createResult.IsFailure) return createResult.Error;

        var createdListing = createResult.Value;

        if (!Guid.TryParse(userId, out var sellerGuid))
        {
            return ListingErrors.Unauthorized;
        }

        var seller = await _userRepository.GetByIdAsNoTrackingAsync(
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

    private async Task<Result<ListingImage>> UploadListingImageAsync(string base64Image, bool isPrimary, CancellationToken cancellationToken)
    {
        var uploadResult = await _cloudinaryService.UploadImageAsync(base64Image, cancellationToken);
        if (uploadResult.IsFailure)
            return Result.Failure<ListingImage>(uploadResult.Error);

        return new ListingImage(uploadResult.Value, isPrimary);
    }

    private async Task<Result<IEnumerable<ListingImage>>> UploadImagesAsync(
        IEnumerable<ListingImage>? existingImages,
        IEnumerable<string>? base64Images, 
        CancellationToken cancellationToken)
    {
        var listingImages = new List<ListingImage>();

        if (existingImages != null)
        {
            listingImages.AddRange(existingImages);
        }

        if (base64Images != null)
        {
            bool isFirst = !listingImages.Any();
            foreach (var imgBase64 in base64Images)
            {
                var uploadRes = await UploadListingImageAsync(imgBase64, isFirst, cancellationToken);
                if (uploadRes.IsFailure) return Result.Failure<IEnumerable<ListingImage>>(uploadRes.Error);
                listingImages.Add(uploadRes.Value);
                isFirst = false;
            }
        }

        return listingImages;
    }

    private async Task<Result<VariationImage>> UploadVariationImageAsync(string base64Image, bool isPrimary, CancellationToken cancellationToken)
    {
        var uploadResult = await _cloudinaryService.UploadImageAsync(base64Image, cancellationToken);
        if (uploadResult.IsFailure)
            return Result.Failure<VariationImage>(uploadResult.Error);

        return new VariationImage(uploadResult.Value, isPrimary);
    }

    private async Task<Result<IEnumerable<VariationImage>>> UploadVariationImagesAsync(
        IEnumerable<ListingImage>? existingImages,
        IEnumerable<string>? base64Images, 
        CancellationToken cancellationToken)
    {
        var variationImages = new List<VariationImage>();

        if (existingImages != null)
        {
            variationImages.AddRange(existingImages.Select(i => new VariationImage(i.Url, i.IsPrimary)));
        }

        if (base64Images != null)
        {
            bool isFirst = !variationImages.Any();
            foreach (var imgBase64 in base64Images)
            {
                var uploadRes = await UploadVariationImageAsync(imgBase64, isFirst, cancellationToken);
                if (uploadRes.IsFailure) return Result.Failure<IEnumerable<VariationImage>>(uploadRes.Error);
                variationImages.Add(uploadRes.Value);
                isFirst = false;
            }
        }

        return variationImages;
    }


    private async Task<Result<Listing>> HandleFixedPriceSingleAsync(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics,
        CancellationToken cancellationToken)
    {
        var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, categorySpecifics);
        if (validateResult.IsFailure) return validateResult.Error;

        var imagesRes = await UploadImagesAsync(request.ListingImages, request.Base64Images, cancellationToken);
        if (imagesRes.IsFailure) return imagesRes.Error;

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
            imagesRes.Value,
            request.Quantity!.Value,
            request.ShippingPolicyId,
            request.ReturnPolicyId
        );
        if (listingOrError.IsFailure) return listingOrError.Error;
        return listingOrError.Value;
    }

    private async Task<Result<Listing>> HandleFixedPriceMultiVariationAsync(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics,
        CancellationToken cancellationToken)
    {
        var variations = new List<Variation>();
        foreach (var v in request.Variations ?? [])
        {
            var validateResult = Category.ValidateSpecifics(v.VariationSpecifics, categorySpecifics);
            if (validateResult.IsFailure) return validateResult.Error;

            var imagesRes = await UploadVariationImagesAsync(v.VariationImages, v.Base64Images, cancellationToken);
            if (imagesRes.IsFailure) return imagesRes.Error;

            var variationOrError = Variation.Create(
                v.Sku,
                v.Price,
                v.VariationSpecifics,
                imagesRes.Value,
                v.Quantity
            );
            if (variationOrError.IsFailure) return variationOrError.Error;

            variations.Add(variationOrError.Value);
        }

        var listingImagesRes = await UploadImagesAsync(request.ListingImages, request.Base64Images, cancellationToken);
        if (listingImagesRes.IsFailure) return listingImagesRes.Error;

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
            variations,
            listingImagesRes.Value,
            request.ShippingPolicyId,
            request.ReturnPolicyId
        );
        if (listingOrError.IsFailure) return listingOrError.Error;
        return listingOrError.Value;
    }

    private async Task<Result<Listing>> HandleAuctionAsync(
        CreateListingCommand request,
        IEnumerable<CategorySpecific> categorySpecifics,
        CancellationToken cancellationToken)
    {
        var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, categorySpecifics);
        if (validateResult.IsFailure) return validateResult.Error;

        var imagesRes = await UploadImagesAsync(request.ListingImages, request.Base64Images, cancellationToken);
        if (imagesRes.IsFailure) return imagesRes.Error;

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
            imagesRes.Value,
            request.ShippingPolicyId,
            request.ReturnPolicyId
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