using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Commands;

internal sealed class ActivateShippingDiscountCommandHandler : ICommandHandler<ActivateShippingDiscountCommand>
{
    private readonly IShippingDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateShippingDiscountCommandHandler(
        IShippingDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateShippingDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);

        if (discount == null)
            return Error.Failure("ShippingDiscount.NotFound", "Shipping discount not found");

        if (discount.IsActive)
            return Error.Failure("ShippingDiscount.AlreadyActive", "Shipping discount is already active");

        discount.Activate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
