using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Services;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Categories.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record UpdateListingCommand(
    Guid ListingId,
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
    bool AllowOffers,
    decimal? MinimumOffer,
    decimal? AutoAcceptOffer,
    bool IsDraft,
    DateTime? ScheduledStartTime = null
) : ICommand;

public sealed class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");
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
        RuleFor(x => x.Duration)
            .IsInEnum().WithMessage("Invalid listing duration.");
    }
}

public sealed class UpdateListingCommandHandler : ICommandHandler<UpdateListingCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPriceIncreaseValidator _priceIncreaseValidator;

    public UpdateListingCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IListingRepository listingRepository,
        IUserContext userContext,
        IPriceIncreaseValidator priceIncreaseValidator)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
        _priceIncreaseValidator = priceIncreaseValidator;
    }

    public async Task<Result> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return ListingErrors.NotFound;

        if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
        {
            return ListingErrors.Unauthorized;
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return CategoryErrors.NotFound;
        if (!category.IsLeaf)
            return CategoryErrors.NotLeaf;

        if (request.Format != listing.Format)
        {
            var formatChangeResult = listing.ChangeFormat(request.Format);
            if (formatChangeResult.IsFailure) return formatChangeResult.Error;
        }

        if (listing is FixedPriceListing fixedPriceListing)
        {
            if (request.Type.HasValue)
            {
                var setTypeResult = fixedPriceListing.SetType(request.Type.Value);
                if (setTypeResult.IsFailure) return setTypeResult.Error;
            }

            var offerSettingsResult = fixedPriceListing.UpdateOfferSettings(
                request.AllowOffers,
                request.MinimumOffer,
                request.AutoAcceptOffer);
            if (offerSettingsResult.IsFailure) return offerSettingsResult.Error;
        }

        var updateCommonResult = listing.UpdateCommon(
            request.Title,
            request.Sku,
            request.ListingDescription,
            request.CategoryId,
            request.ConditionId,
            request.ConditionDescription,
            request.ItemSpecifics,
            request.ListingImages ?? []);
        if (updateCommonResult.IsFailure) return updateCommonResult.Error;

        switch (listing)
        {
            case FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.Single:
                {
                    var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, category.CategorySpecifics);
                    if (validateResult.IsFailure) return validateResult.Error;
                    
                    // Validate price increase if price is changing
                    if (request.Price.HasValue && request.Price.Value != fixedPrice.Pricing.Price)
                    {
                        var priceValidation = await _priceIncreaseValidator.ValidatePriceChange(
                            listing.Id, 
                            request.Price.Value, 
                            cancellationToken);
                        if (priceValidation.IsFailure) return priceValidation.Error;
                    }
                    
                    var pricingResult = fixedPrice.UpdatePricing(request.Price!.Value, request.Quantity!.Value);
                    if (pricingResult.IsFailure) return pricingResult.Error;
                    break;
                }

            case FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.MultiVariation:
                {
                    var variations = new List<Variation>();
                    foreach (var v in request.Variations ?? Enumerable.Empty<VariationDto>())
                    {
                        var variationSpecificsValidation = Category.ValidateSpecifics(v.VariationSpecifics, category.CategorySpecifics);
                        if (variationSpecificsValidation.IsFailure) return variationSpecificsValidation.Error;

                        // Validate price increase for each variation
                        var priceValidation = await _priceIncreaseValidator.ValidatePriceChange(
                            listing.Id, 
                            v.Price, 
                            cancellationToken);
                        if (priceValidation.IsFailure) return priceValidation.Error;

                        var variationOrError = Variation.Create(
                            v.Sku,
                            v.Price,
                            v.VariationSpecifics,
                            v.VariationImages ?? [],
                            v.Quantity);
                        if (variationOrError.IsFailure) return variationOrError.Error;
                        variations.Add(variationOrError.Value);
                    }

                    var updateVariationsResult = fixedPrice.UpdateVariations(variations);
                    if (updateVariationsResult.IsFailure) return updateVariationsResult.Error;
                    break;
                }

            case AuctionListing auction:
                {
                    var validateResult = Category.ValidateSpecifics(request.ItemSpecifics, category.CategorySpecifics);
                    if (validateResult.IsFailure) return validateResult.Error;
                    var updatePricingResult = auction.UpdatePricing(
                    request.StartPrice!.Value,
                    request.ReservePrice,
                    request.BuyItNowPrice,
                    request.Duration);
                    if (updatePricingResult.IsFailure) return updatePricingResult.Error;
                    break;
                }
        }

        if (request.IsDraft)
        {
            var saveAsDraftResult = listing.Draft();
            if (saveAsDraftResult.IsFailure) return saveAsDraftResult.Error;
        }
        else if (request.ScheduledStartTime.HasValue)
        {
            var scheduleResult = listing.Schedule(request.ScheduledStartTime.Value);
            if (scheduleResult.IsFailure) return scheduleResult.Error;
        }
        else
        {
            var activateResult = listing.Activate();
            if (activateResult.IsFailure) return activateResult.Error;
        }

        _listingRepository.Update(listing);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


