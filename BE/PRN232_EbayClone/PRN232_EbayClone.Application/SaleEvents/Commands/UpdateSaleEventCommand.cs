using PRN232_EbayClone.Application.Abstractions.Authentication;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record UpdateSaleEventCommand(
    Guid SaleEventId,
    string? Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate,
    string? BuyerMessageLabel,
    bool? OfferFreeShipping,
    bool? BlockPriceIncreaseRevisions,
    bool? IncludeSkippedItems
) : ICommand;

public sealed class UpdateSaleEventCommandValidator : AbstractValidator<UpdateSaleEventCommand>
{
    public UpdateSaleEventCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.Name != null)
            .WithMessage("Sale event name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null)
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.BuyerMessageLabel)
            .MaximumLength(200)
            .When(x => x.BuyerMessageLabel != null)
            .WithMessage("Buyer message label cannot exceed 200 characters");

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate.Value < x.EndDate.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be before end date");
    }
}

public sealed class UpdateSaleEventCommandHandler : ICommandHandler<UpdateSaleEventCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleEventCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSaleEventCommand request, CancellationToken cancellationToken)
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

        try
        {
            saleEvent.Update(
                request.Name,
                request.Description,
                request.StartDate,
                request.EndDate,
                request.BuyerMessageLabel,
                request.OfferFreeShipping,
                request.BlockPriceIncreaseRevisions,
                request.IncludeSkippedItems);

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
