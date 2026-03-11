using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Coupons.Errors;

namespace PRN232_EbayClone.Application.Coupons.Queries;

public sealed record GetCouponByCodeQuery(string Code) : IQuery<CouponDetailsDto>;

public sealed record CouponDetailsDto(
    Guid Id,
    Guid CouponTypeId,
    Guid? CategoryId,
    Guid? SellerId,
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
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<CouponConditionDto> Conditions
);

public sealed record CouponConditionDto(
    Guid Id,
    int? BuyQuantity,
    int? GetQuantity,
    decimal? GetDiscountPercent,
    decimal? SaveEveryAmount,
    int? SaveEveryItems,
    string? ConditionDescription
);

public sealed class GetCouponByCodeQueryHandler : IQueryHandler<GetCouponByCodeQuery, CouponDetailsDto>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUserContext _userContext;

    public GetCouponByCodeQueryHandler(ICouponRepository couponRepository, IUserContext userContext)
    {
        _couponRepository = couponRepository;
        _userContext = userContext;
    }

    public async Task<Result<CouponDetailsDto>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code?.Trim();
        if (string.IsNullOrWhiteSpace(code))
        {
            return CouponErrors.EmptyCode;
        }

        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var currentUserId))
        {
            return CouponErrors.Unauthorized;
        }

        var coupon = await _couponRepository.GetByCodeAsync(code, cancellationToken);
        if (coupon is null)
        {
            return CouponErrors.NotFound;
        }

        var sellerId = coupon.SellerId?.Value;
        if (sellerId.HasValue && sellerId.Value != currentUserId)
        {
            return CouponErrors.Unauthorized;
        }

        var conditions = coupon.Conditions
            .Select(condition => new CouponConditionDto(
                condition.Id,
                condition.BuyQuantity,
                condition.GetQuantity,
                condition.GetDiscountPercent,
                condition.SaveEveryAmount,
                condition.SaveEveryItems,
                condition.ConditionDescription))
            .ToList();

        var dto = new CouponDetailsDto(
            coupon.Id,
            coupon.CouponTypeId,
            coupon.CategoryId,
            sellerId,
            coupon.Name,
            coupon.Code,
            coupon.DiscountValue,
            coupon.DiscountUnit,
            coupon.MaxDiscount,
            coupon.StartDate,
            coupon.EndDate,
            coupon.UsageLimit,
            coupon.UsagePerUser,
            coupon.MinimumOrderValue,
            coupon.ApplicablePriceMin,
            coupon.ApplicablePriceMax,
            coupon.IsActive,
            coupon.CreatedAt,
            coupon.UpdatedAt,
            conditions);

        return dto;
    }
}
