using PRN232_EbayClone.Application.Abstractions.Authentication;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record RemoveListingAssignmentCommand(
    Guid SaleEventId,
    Guid ListingId
) : ICommand;

public sealed class RemoveListingAssignmentCommandValidator : AbstractValidator<RemoveListingAssignmentCommand>
{
    public RemoveListingAssignmentCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");

        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required");
    }
}

public sealed class RemoveListingAssignmentCommandHandler : ICommandHandler<RemoveListingAssignmentCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveListingAssignmentCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveListingAssignmentCommand request, CancellationToken cancellationToken)
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
            saleEvent.RemoveListingAssignment(request.ListingId);

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
