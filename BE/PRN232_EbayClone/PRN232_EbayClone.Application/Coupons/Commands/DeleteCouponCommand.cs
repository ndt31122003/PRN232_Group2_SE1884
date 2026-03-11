using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Coupons.Commands;

public sealed record DeleteCouponCommand(Guid CouponId) : ICommand;

public sealed class DeleteCouponCommandHandler : ICommandHandler<DeleteCouponCommand>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(
        ICouponRepository couponRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        if (request.CouponId == Guid.Empty)
        {
            return CouponErrors.NotFound;
        }

        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var sellerId))
        {
            return CouponErrors.Unauthorized;
        }

        var coupon = await _couponRepository.GetByIdAsync(request.CouponId, cancellationToken);
        if (coupon is null)
        {
            return CouponErrors.NotFound;
        }

        if (coupon.SellerId?.Value != sellerId)
        {
            return CouponErrors.Unauthorized;
        }

        _couponRepository.Remove(coupon);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
