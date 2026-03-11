using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.SaleEvents.Enums;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record UpdateSaleEventStatusCommand(Guid SaleEventId, SaleEventStatus Status) : ICommand;

public sealed class UpdateSaleEventStatusCommandValidator : AbstractValidator<UpdateSaleEventStatusCommand>
{
    public UpdateSaleEventStatusCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}

public sealed class UpdateSaleEventStatusCommandHandler : ICommandHandler<UpdateSaleEventStatusCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleEventStatusCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSaleEventStatusCommand request, CancellationToken cancellationToken)
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

        var updateResult = saleEvent.UpdateStatus(request.Status, DateTime.UtcNow);
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        _saleEventRepository.Update(saleEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
