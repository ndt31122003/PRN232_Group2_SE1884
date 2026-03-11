using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Coupons.Queries;

public sealed record GetSellerCouponsQuery() : IQuery<IReadOnlyCollection<CouponSummaryDto>>;

public sealed record CouponSummaryDto(
    Guid Id,
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
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed class GetSellerCouponsQueryHandler : IQueryHandler<GetSellerCouponsQuery, IReadOnlyCollection<CouponSummaryDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUserContext _userContext;

    public GetSellerCouponsQueryHandler(ICouponRepository couponRepository, IUserContext userContext)
    {
        _couponRepository = couponRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyCollection<CouponSummaryDto>>> Handle(GetSellerCouponsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var sellerId))
        {
            return CouponErrors.Unauthorized;
        }

        var coupons = await _couponRepository.GetSellerCouponsAsync(sellerId, cancellationToken);

        var summaries = coupons
            .Select(coupon => new CouponSummaryDto(
                coupon.Id,
                coupon.CouponTypeId,
                coupon.CategoryId,
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
                coupon.UpdatedAt))
            .ToList();

        return summaries;
    }
}
