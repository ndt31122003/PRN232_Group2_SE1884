using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.Coupons.Commands;
using PRN232_EbayClone.Application.Coupons.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/coupons")]
public sealed class CouponsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetSellerCoupons(CancellationToken cancellationToken)
        => SendAsync(new GetSellerCouponsQuery(), cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateCoupon([FromBody] CreateSellerCouponCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("{code}")]
    public Task<IActionResult> GetCouponByCode([FromRoute] string code, CancellationToken cancellationToken)
        => SendAsync(new GetCouponByCodeQuery(code), cancellationToken);

    [HttpPatch("{couponId:guid}/status")]
    public Task<IActionResult> UpdateCouponStatus([FromRoute] Guid couponId, [FromBody] UpdateCouponStatusRequest request, CancellationToken cancellationToken)
        => SendAsync(new UpdateCouponStatusCommand(couponId, request.IsActive), cancellationToken);

    [HttpDelete("{couponId:guid}")]
    public Task<IActionResult> DeleteCoupon([FromRoute] Guid couponId, CancellationToken cancellationToken)
        => SendAsync(new DeleteCouponCommand(couponId), cancellationToken);
}

public sealed record UpdateCouponStatusRequest(bool IsActive);
