using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

internal sealed class CreateSpendBasedDiscountCommandHandler : ICommandHandler<CreateSpendBasedDiscountCommand, Guid>
{
    private readonly IOrderDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpendBasedDiscountCommandHandler(
        IOrderDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSpendBasedDiscountCommand request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);

        var discountResult = OrderDiscount.CreateSpendBased(
            sellerId,
            request.Name,
            request.Description,
            request.DiscountValue,
            request.DiscountUnit,
            request.MaxDiscount,
            request.ThresholdAmount,
            request.StartDate,
            request.EndDate);

        if (discountResult.IsFailure)
            return discountResult.Error;

        var discount = discountResult.Value;

        // Add tiers if provided
        if (request.Tiers != null)
        {
            foreach (var tier in request.Tiers.OrderBy(t => t.ThresholdValue))
            {
                var tierResult = discount.AddTier(tier.ThresholdValue, tier.DiscountValue);
                if (tierResult.IsFailure)
                    return tierResult.Error;
            }
        }

        // Add item rules if provided
        if (request.IncludedItemIds != null)
        {
            foreach (var itemId in request.IncludedItemIds)
            {
                var result = discount.AddItemRule(itemId, isExclusion: false);
                if (result.IsFailure)
                    return result.Error;
            }
        }

        if (request.ExcludedItemIds != null)
        {
            foreach (var itemId in request.ExcludedItemIds)
            {
                var result = discount.AddItemRule(itemId, isExclusion: true);
                if (result.IsFailure)
                    return result.Error;
            }
        }

        // Add category rules if provided
        if (request.IncludedCategoryIds != null)
        {
            foreach (var categoryId in request.IncludedCategoryIds)
            {
                var result = discount.AddCategoryRule(categoryId, isExclusion: false);
                if (result.IsFailure)
                    return result.Error;
            }
        }

        if (request.ExcludedCategoryIds != null)
        {
            foreach (var categoryId in request.ExcludedCategoryIds)
            {
                var result = discount.AddCategoryRule(categoryId, isExclusion: true);
                if (result.IsFailure)
                    return result.Error;
            }
        }

        await _repository.AddAsync(discount, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(discount.Id);
    }
}
