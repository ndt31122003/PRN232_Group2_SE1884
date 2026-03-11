using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Coupons;
using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Coupons.Commands;

public sealed record CreateSellerCouponCommand(
    Guid CouponTypeId,
    Guid? CategoryId,
    string Name,
    string Code,
    decimal DiscountValue,
    CouponDiscountUnit DiscountUnit,
    decimal? MaxDiscount,
    DateTime StartDate,
    DateTime EndDate,
    int? UsageLimit,
    int? UsagePerUser,
    decimal? MinimumOrderValue,
    decimal? ApplicablePriceMin,
    decimal? ApplicablePriceMax,
    bool IsActive,
    IReadOnlyList<CreateCouponConditionRequest>? Conditions
) : ICommand<Guid>;

public sealed record CreateCouponConditionRequest(
    int? BuyQuantity,
    int? GetQuantity,
    decimal? GetDiscountPercent,
    decimal? SaveEveryAmount,
    int? SaveEveryItems,
    string? ConditionDescription
);

public sealed class CreateSellerCouponCommandValidator : AbstractValidator<CreateSellerCouponCommand>
{
    public CreateSellerCouponCommandValidator()
    {
        RuleFor(x => x.CouponTypeId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => Regex.IsMatch(code, "^[A-Za-z0-9-]+$"))
            .WithMessage("Coupon code must contain only letters, digits, or hyphens.");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0);

        RuleFor(x => x.DiscountUnit)
            .IsInEnum();

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.MaxDiscount)
            .GreaterThan(0)
            .When(x => x.MaxDiscount.HasValue);

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .When(x => x.UsageLimit.HasValue);

        RuleFor(x => x.UsagePerUser)
            .GreaterThan(0)
            .When(x => x.UsagePerUser.HasValue);

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinimumOrderValue.HasValue);

        RuleFor(x => x.ApplicablePriceMin)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ApplicablePriceMin.HasValue);

        RuleFor(x => x.ApplicablePriceMax)
            .GreaterThanOrEqualTo(x => x.ApplicablePriceMin!.Value)
            .When(x => x.ApplicablePriceMin.HasValue && x.ApplicablePriceMax.HasValue);

        RuleForEach(x => x.Conditions)
            .SetValidator(new CreateCouponConditionRequestValidator())
            .When(x => x.Conditions is not null);
    }

    private sealed class CreateCouponConditionRequestValidator : AbstractValidator<CreateCouponConditionRequest>
    {
        public CreateCouponConditionRequestValidator()
        {
            RuleFor(x => x.BuyQuantity)
                .GreaterThanOrEqualTo(0)
                .When(x => x.BuyQuantity.HasValue);

            RuleFor(x => x.GetQuantity)
                .GreaterThanOrEqualTo(0)
                .When(x => x.GetQuantity.HasValue);

            RuleFor(x => x.GetDiscountPercent)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .When(x => x.GetDiscountPercent.HasValue);

            RuleFor(x => x.SaveEveryAmount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SaveEveryAmount.HasValue);

            RuleFor(x => x.SaveEveryItems)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SaveEveryItems.HasValue);
        }
    }
}

public sealed class CreateSellerCouponCommandHandler : ICommandHandler<CreateSellerCouponCommand, Guid>
{
    private readonly ICouponRepository _couponRepository;
    private readonly ICouponTypeRepository _couponTypeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSellerCouponCommandHandler(
        ICouponRepository couponRepository,
        ICouponTypeRepository couponTypeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _couponTypeRepository = couponTypeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSellerCouponCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return CouponErrors.Unauthorized;
        }

        var couponType = await _couponTypeRepository.GetByIdAsync(request.CouponTypeId, cancellationToken);
        if (couponType is null)
        {
            return CouponErrors.CouponTypeNotFound;
        }

        if (!couponType.IsActive)
        {
            return CouponErrors.CouponTypeInactive;
        }

        var codeExists = await _couponRepository.CodeExistsAsync(request.Code, cancellationToken);
        if (codeExists)
        {
            return CouponErrors.CodeAlreadyExists;
        }

        var typeValidationResult = ValidateTypeSpecificData(request);
        if (typeValidationResult.IsFailure)
        {
            return typeValidationResult.Error;
        }

        var sellerId = new UserId(sellerGuid);

        var couponOrError = Coupon.Create(
            request.CouponTypeId,
            request.CategoryId,
            sellerId,
            request.Name,
            request.Code,
            request.DiscountValue,
            request.DiscountUnit,
            request.MaxDiscount,
            request.StartDate,
            request.EndDate,
            request.UsageLimit,
            request.UsagePerUser,
            request.MinimumOrderValue,
            request.ApplicablePriceMin,
            request.ApplicablePriceMax,
            request.IsActive);

        if (couponOrError.IsFailure)
        {
            return couponOrError.Error;
        }

        var coupon = couponOrError.Value;

        foreach (var condition in typeValidationResult.Value)
        {
            var conditionOrError = CouponCondition.Create(
                coupon.Id,
                condition.BuyQuantity,
                condition.GetQuantity,
                condition.GetDiscountPercent,
                condition.SaveEveryAmount,
                condition.SaveEveryItems,
                condition.ConditionDescription);

            if (conditionOrError.IsFailure)
            {
                return conditionOrError.Error;
            }

            var attachResult = coupon.AddCondition(conditionOrError.Value);
            if (attachResult.IsFailure)
            {
                return attachResult.Error;
            }
        }

        _couponRepository.Add(coupon);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(coupon.Id);
    }

    private static Result<IReadOnlyList<ConditionValues>> ValidateTypeSpecificData(CreateSellerCouponCommand request)
    {
        var rawConditions = (request.Conditions ?? Array.Empty<CreateCouponConditionRequest>())
            .Select(c => new ConditionValues(
                c.BuyQuantity,
                c.GetQuantity,
                c.GetDiscountPercent,
                c.SaveEveryAmount,
                c.SaveEveryItems,
                c.ConditionDescription))
            .ToList();

        static Result<IReadOnlyList<ConditionValues>> Fail(Error error) =>
            Result.Failure<IReadOnlyList<ConditionValues>>(error);

        static Result<IReadOnlyList<ConditionValues>> Ok(params ConditionValues[] conditions) =>
            Result.Success<IReadOnlyList<ConditionValues>>(conditions.Length == 0
                ? Array.Empty<ConditionValues>()
                : conditions);

        switch (request.CouponTypeId)
        {
            case var id when id == CouponTypeConstants.ExtraPercentOff:
                if (request.DiscountUnit != CouponDiscountUnit.Percent)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count > 0)
                {
                    return Fail(CouponErrors.ConditionsNotAllowed);
                }

                return Ok();

            case var id when id == CouponTypeConstants.ExtraPercentOffYOrMoreItems:
                if (request.DiscountUnit != CouponDiscountUnit.Percent)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var percentThreshold = rawConditions[0];
                if (percentThreshold.SaveEveryItems is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                return Ok(percentThreshold);

            case var id when id == CouponTypeConstants.ExtraAmountOffAmountOrMore:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (request.MinimumOrderValue is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count > 0)
                {
                    return Fail(CouponErrors.ConditionsNotAllowed);
                }

                return Ok();

            case var id when id == CouponTypeConstants.BuyXGetYAtPercentOff:
                if (request.DiscountUnit != CouponDiscountUnit.Percent)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var buyGetPercent = rawConditions[0];
                if (buyGetPercent.BuyQuantity is null or <= 0 ||
                    buyGetPercent.GetQuantity is null or <= 0 ||
                    buyGetPercent.GetDiscountPercent is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                return Ok(buyGetPercent);

            case var id when id == CouponTypeConstants.BuyXGetYFree:
                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var buyGetFree = rawConditions[0];
                if (buyGetFree.BuyQuantity is null or <= 0 ||
                    buyGetFree.GetQuantity is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                var freeCondition = buyGetFree with { GetDiscountPercent = 100m };
                return Ok(freeCondition);

            case var id when id == CouponTypeConstants.ExtraPercentOffAmountOrMore:
                if (request.DiscountUnit != CouponDiscountUnit.Percent)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (request.MinimumOrderValue is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count > 0)
                {
                    return Fail(CouponErrors.ConditionsNotAllowed);
                }

                return Ok();

            case var id when id == CouponTypeConstants.ExtraAmountOffItemsThreshold:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var itemsThreshold = rawConditions[0];
                if (itemsThreshold.SaveEveryItems is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                return Ok(itemsThreshold);

            case var id when id == CouponTypeConstants.ExtraAmountOff:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count > 0)
                {
                    return Fail(CouponErrors.ConditionsNotAllowed);
                }

                return Ok();

            case var id when id == CouponTypeConstants.ExtraAmountOffEachItem:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count > 0)
                {
                    return Fail(CouponErrors.ConditionsNotAllowed);
                }

                return Ok();

            case var id when id == CouponTypeConstants.SaveAmountForEveryItems:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var perItems = rawConditions[0];
                if (perItems.SaveEveryAmount is null or <= 0 ||
                    perItems.SaveEveryItems is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                return Ok(perItems);

            case var id when id == CouponTypeConstants.SaveAmountForEveryAmount:
                if (request.DiscountUnit != CouponDiscountUnit.Amount)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                if (rawConditions.Count != 1)
                {
                    return Fail(CouponErrors.ConditionsRequired);
                }

                var perAmount = rawConditions[0];
                if (perAmount.SaveEveryAmount is null or <= 0)
                {
                    return Fail(CouponErrors.InvalidTypeConfiguration);
                }

                return Ok(perAmount);

            default:
                return Fail(CouponErrors.InvalidTypeConfiguration);
        }
    }

    private sealed record ConditionValues(
        int? BuyQuantity,
        int? GetQuantity,
        decimal? GetDiscountPercent,
        decimal? SaveEveryAmount,
        int? SaveEveryItems,
        string? ConditionDescription);
}
