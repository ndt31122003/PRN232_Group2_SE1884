using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Commands;

internal sealed class CreateShippingDiscountCommandHandler : ICommandHandler<CreateShippingDiscountCommand, Guid>
{
    private readonly IShippingDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShippingDiscountCommandHandler(
        IShippingDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateShippingDiscountCommand request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);

        var discountResult = ShippingDiscount.Create(
            sellerId,
            request.Name,
            request.Description,
            request.DiscountValue,
            request.DiscountUnit,
            request.IsFreeShipping,
            request.MinimumOrderValue,
            request.StartDate,
            request.EndDate);

        if (discountResult.IsFailure)
            return discountResult.Error;

        await _repository.AddAsync(discountResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(discountResult.Value.Id);
    }
}
