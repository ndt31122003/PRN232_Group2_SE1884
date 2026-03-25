using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Commands;

internal sealed class DeleteShippingDiscountCommandHandler : ICommandHandler<DeleteShippingDiscountCommand>
{
    private readonly IShippingDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShippingDiscountCommandHandler(
        IShippingDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteShippingDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);

        if (discount == null)
            return Error.Failure("ShippingDiscount.NotFound", "Shipping discount not found");

        await _repository.DeleteAsync(request.DiscountId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
