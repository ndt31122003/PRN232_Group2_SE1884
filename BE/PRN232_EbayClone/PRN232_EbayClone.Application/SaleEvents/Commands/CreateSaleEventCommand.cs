using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Discounts.ValueObjects;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record CreateSaleEventCommand(
    string Name,
    string? Description,
    string? BuyerMessageLabel,
    SaleEventMode Mode,
    DateTime StartDate,
    DateTime EndDate,
    bool OfferFreeShipping,
    bool BlockPriceIncreaseRevisions,
    bool IncludeSkippedItems,
    decimal? HighlightPercentage,
    IReadOnlyList<CreateSaleEventTierRequest>? Tiers
) : ICommand<Guid>;

public sealed record CreateSaleEventTierRequest(
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    int Priority,
    string? Label,
    IReadOnlyList<Guid> ListingIds
);

public sealed class CreateSaleEventCommandValidator : AbstractValidator<CreateSaleEventCommand>
{
    public CreateSaleEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Sale event name is required")
            .MaximumLength(200)
            .WithMessage("Sale event name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null)
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.BuyerMessageLabel)
            .MaximumLength(200)
            .When(x => x.BuyerMessageLabel != null)
            .WithMessage("Buyer message label cannot exceed 200 characters");

        RuleFor(x => x.Mode)
            .IsInEnum()
            .WithMessage("Invalid sale event mode");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date");

        RuleFor(x => x.HighlightPercentage)
            .InclusiveBetween(0.01m, 90m)
            .When(x => x.Mode == SaleEventMode.SaleEventOnly && x.HighlightPercentage.HasValue)
            .WithMessage("Highlight percentage must be between 0.01 and 90");

        RuleFor(x => x.Tiers)
            .NotEmpty()
            .When(x => x.Mode == SaleEventMode.DiscountAndSaleEvent)
            .WithMessage("At least one discount tier is required for DiscountAndSaleEvent mode");

        RuleFor(x => x.Tiers)
            .Must(tiers => tiers == null || tiers.Count <= 10)
            .WithMessage("Cannot have more than 10 discount tiers");

        RuleFor(x => x.Tiers)
            .Must(tiers => tiers == null || tiers.Select(t => t.Priority).Distinct().Count() == tiers.Count)
            .When(x => x.Tiers != null && x.Tiers.Any())
            .WithMessage("Tier priorities must be unique");

        RuleForEach(x => x.Tiers)
            .SetValidator(new CreateSaleEventTierRequestValidator())
            .When(x => x.Tiers != null);
    }

    private sealed class CreateSaleEventTierRequestValidator : AbstractValidator<CreateSaleEventTierRequest>
    {
        public CreateSaleEventTierRequestValidator()
        {
            RuleFor(x => x.Priority)
                .GreaterThan(0)
                .WithMessage("Tier priority must be greater than 0");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0)
                .WithMessage("Discount value must be greater than 0");

            RuleFor(x => x.DiscountValue)
                .InclusiveBetween(0.01m, 90m)
                .When(x => x.DiscountType == SaleEventDiscountType.Percent)
                .WithMessage("Percentage discount must be between 0.01 and 90");

            RuleFor(x => x.Label)
                .MaximumLength(100)
                .When(x => x.Label != null)
                .WithMessage("Tier label cannot exceed 100 characters");

            RuleFor(x => x.ListingIds)
                .NotEmpty()
                .WithMessage("At least one listing must be assigned to the tier");
        }
    }
}

public sealed class CreateSaleEventCommandHandler : ICommandHandler<CreateSaleEventCommand, Guid>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleEventCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSaleEventCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return new Error("SaleEvent.Unauthorized", "User is not authorized");
        }

        // Convert tier requests to tier definitions
        List<SaleEventDiscountTierDefinition>? tierDefinitions = null;
        if (request.Tiers != null && request.Tiers.Any())
        {
            tierDefinitions = request.Tiers
                .Select(t => new SaleEventDiscountTierDefinition(
                    t.DiscountType,
                    t.DiscountValue,
                    t.Priority,
                    t.Label,
                    t.ListingIds.ToList()))
                .ToList();
        }

        // Create sale event using domain factory method
        try
        {
            var saleEvent = SaleEvent.Create(
                request.Name,
                sellerGuid,
                request.StartDate,
                request.EndDate,
                request.Mode,
                request.HighlightPercentage,
                request.OfferFreeShipping,
                request.BlockPriceIncreaseRevisions,
                request.IncludeSkippedItems,
                request.Description,
                request.BuyerMessageLabel,
                tierDefinitions);

            await _saleEventRepository.AddAsync(saleEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(saleEvent.Id);
        }
        catch (ArgumentException ex)
        {
            return new Error("SaleEvent.ValidationFailed", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return new Error("SaleEvent.OperationFailed", ex.Message);
        }
    }
}
