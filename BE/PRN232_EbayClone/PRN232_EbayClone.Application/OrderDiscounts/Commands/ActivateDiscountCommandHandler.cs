using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

internal sealed class ActivateDiscountCommandHandler : ICommandHandler<ActivateDiscountCommand>
{
    private readonly IOrderDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateDiscountCommandHandler(
        IOrderDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);
        
        if (discount == null)
            return Error.Failure("OrderDiscount.NotFound", "Discount not found");

        if (discount.IsActive)
            return Error.Failure("OrderDiscount.AlreadyActive", "Discount is already active");

        discount.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
