using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Services;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record AssignListingsToTierCommand(
    Guid SaleEventId,
    Guid TierId,
    IReadOnlyList<Guid> ListingIds
) : ICommand;

public sealed class AssignListingsToTierCommandValidator : AbstractValidator<AssignListingsToTierCommand>
{
    public AssignListingsToTierCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");

        RuleFor(x => x.TierId)
            .NotEmpty()
            .WithMessage("Tier ID is required");

        RuleFor(x => x.ListingIds)
            .NotEmpty()
            .WithMessage("At least one listing ID is required");

        RuleFor(x => x.ListingIds)
            .Must(ids => ids.Count <= 1000)
            .WithMessage("Cannot assign more than 1000 listings at once");
    }
}

public sealed class AssignListingsToTierCommandHandler : ICommandHandler<AssignListingsToTierCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly ISaleEventEligibilityValidator _eligibilityValidator;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AssignListingsToTierCommandHandler(
        ISaleEventRepository saleEventRepository,
        ISaleEventEligibilityValidator eligibilityValidator,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _eligibilityValidator = eligibilityValidator;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignListingsToTierCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return new Error("SaleEvent.Unauthorized", "User is not authorized");
        }

        var saleEvent = await _saleEventRepository.GetByIdAsync(request.SaleEventId, cancellationToken);
        if (saleEvent == null)
        {
            return new Error("SaleEvent.NotFound", "Sale event not found");
        }

        // Verify ownership
        if (saleEvent.SellerId != sellerGuid)
        {
            return new Error("SaleEvent.Unauthorized", "User does not own this sale event");
        }

        // Validate each listing before assignment
        var validListings = new List<Guid>();
        var errors = new List<string>();

        foreach (var listingId in request.ListingIds)
        {
            var validationResult = await _eligibilityValidator.ValidateListingForTierAssignment(
                listingId,
                request.SaleEventId,
                sellerGuid,
                cancellationToken);

            if (validationResult.IsSuccess)
            {
                validListings.Add(listingId);
            }
            else
            {
                errors.Add($"Listing {listingId}: {validationResult.Error.Description}");
            }
        }

        if (!validListings.Any())
        {
            return new Error(
                "SaleEvent.NoValidListings",
                $"No valid listings to assign. Errors: {string.Join("; ", errors)}");
        }

        try
        {
            // Bulk assign valid listings
            var assignments = validListings.Select(id => (id, request.TierId)).ToList();
            saleEvent.BulkAssignListings(assignments);

            await _saleEventRepository.UpdateAsync(saleEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
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
