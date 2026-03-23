using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

internal sealed class DeactivateDiscountCommandHandler : ICommandHandler<DeactivateDiscountCommand>
{
    private readonly IOrderDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateDiscountCommandHandler(
        IOrderDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);
        
        if (discount == null)
            return Error.Failure("OrderDiscount.NotFound", "Discount not found");

        if (!discount.IsActive)
            return Error.Failure("OrderDiscount.AlreadyInactive", "Discount is already inactive");

        discount.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
