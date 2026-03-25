using PRN232_EbayClone.Application.Abstractions.Authentication;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record DeleteSaleEventCommand(Guid SaleEventId) : ICommand;

public sealed class DeleteSaleEventCommandValidator : AbstractValidator<DeleteSaleEventCommand>
{
    public DeleteSaleEventCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");
    }
}

public sealed class DeleteSaleEventCommandHandler : ICommandHandler<DeleteSaleEventCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSaleEventCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteSaleEventCommand request, CancellationToken cancellationToken)
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

        // Check if sale event has been activated
        var hasBeenApplied = await _saleEventRepository.HasBeenAppliedToOrdersAsync(
            request.SaleEventId,
            cancellationToken);

        if (hasBeenApplied)
        {
            return new Error(
                "SaleEvent.CannotDelete",
                "Cannot delete a sale event that has been applied to orders");
        }

        await _saleEventRepository.DeleteAsync(saleEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
