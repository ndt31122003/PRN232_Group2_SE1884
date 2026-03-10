using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record DeleteSaleEventCommand(Guid SaleEventId) : ICommand;

public sealed class DeleteSaleEventCommandValidator : AbstractValidator<DeleteSaleEventCommand>
{
    public DeleteSaleEventCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEqual(Guid.Empty);
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
            return SaleEventErrors.Unauthorized;
        }

        var sellerIdString = sellerGuid.ToString();
        var saleEvent = await _saleEventRepository.GetByIdForSellerTrackingAsync(request.SaleEventId, sellerIdString, cancellationToken);
        if (saleEvent is null)
        {
            return SaleEventErrors.NotFound;
        }

        _saleEventRepository.Remove(saleEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
