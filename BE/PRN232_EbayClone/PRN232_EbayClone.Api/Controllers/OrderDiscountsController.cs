using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.OrderDiscounts.Commands;
using PRN232_EbayClone.Application.OrderDiscounts.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/order-discounts")]
public sealed class OrderDiscountsController(ISender sender) : ApiController(sender)
{
    [HttpPost("spend-based")]
    public Task<IActionResult> CreateSpendBasedDiscount(
        [FromBody] CreateSpendBasedDiscountCommand command,
        CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("quantity-based")]
    public Task<IActionResult> CreateQuantityBasedDiscount(
        [FromBody] CreateQuantityBasedDiscountCommand command,
        CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("{discountId:guid}")]
    public Task<IActionResult> GetDiscountById(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new GetOrderDiscountByIdQuery(discountId), cancellationToken);

    [HttpGet("seller/{sellerId:guid}")]
    public Task<IActionResult> GetDiscountsBySeller(
        [FromRoute] Guid sellerId,
        CancellationToken cancellationToken)
        => SendAsync(new GetOrderDiscountsBySellerQuery(sellerId), cancellationToken);

    [HttpPost("{discountId:guid}/activate")]
    public Task<IActionResult> ActivateDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new ActivateDiscountCommand(discountId), cancellationToken);

    [HttpPost("{discountId:guid}/deactivate")]
    public Task<IActionResult> DeactivateDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new DeactivateDiscountCommand(discountId), cancellationToken);

    [HttpDelete("{discountId:guid}")]
    public Task<IActionResult> DeleteDiscount(
        [FromRoute] Guid discountId,
        CancellationToken cancellationToken)
        => SendAsync(new DeleteDiscountCommand(discountId), cancellationToken);
}
