using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.ShippingDiscounts.Commands;
using PRN232_EbayClone.Application.ShippingDiscounts.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/shipping-discounts")]
public sealed class ShippingDiscountsController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public Task<IActionResult> CreateShippingDiscount(
        [FromBody] CreateShippingDiscountCommand command,
        CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("seller/{sellerId:guid}")]
    public Task<IActionResult> GetDiscountsBySeller(
        [FromRoute] Guid sellerId,
        CancellationToken cancellationToken)
        => SendAsync(new GetShippingDiscountsBySellerQuery(sellerId), cancellationToken);

    [HttpGet("{discountId:guid}")]
    public Task<IActionResult> GetDiscountById(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new GetShippingDiscountByIdQuery(discountId), cancellationToken);

    [HttpPost("{discountId:guid}/activate")]
    public Task<IActionResult> ActivateDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new ActivateShippingDiscountCommand(discountId), cancellationToken);

    [HttpPost("{discountId:guid}/deactivate")]
    public Task<IActionResult> DeactivateDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new DeactivateShippingDiscountCommand(discountId), cancellationToken);

    [HttpDelete("{discountId:guid}")]
    public Task<IActionResult> DeleteDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new DeleteShippingDiscountCommand(discountId), cancellationToken);
}
