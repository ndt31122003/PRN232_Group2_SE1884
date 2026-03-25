using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.VolumePricings.Commands;
using PRN232_EbayClone.Application.VolumePricings.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/volume-pricings")]
public sealed class VolumePricingsController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public Task<IActionResult> CreateVolumePricing(
        [FromBody] CreateVolumePricingCommand command,
        CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("seller/{sellerId:guid}")]
    public Task<IActionResult> GetPricingsBySeller(
        [FromRoute] Guid sellerId,
        CancellationToken cancellationToken)
        => SendAsync(new GetVolumePricingsBySellerQuery(sellerId), cancellationToken);

    [HttpGet("{pricingId:guid}")]
    public Task<IActionResult> GetPricingById(
        [FromRoute] Guid pricingId,
        CancellationToken cancellationToken)
        => SendAsync(new GetVolumePricingByIdQuery(pricingId), cancellationToken);

    [HttpPost("{pricingId:guid}/activate")]
    public Task<IActionResult> ActivatePricing(
        [FromRoute] Guid pricingId,
        CancellationToken cancellationToken)
        => SendAsync(new ActivateVolumePricingCommand(pricingId), cancellationToken);

    [HttpPost("{pricingId:guid}/deactivate")]
    public Task<IActionResult> DeactivatePricing(
        [FromRoute] Guid pricingId,
        CancellationToken cancellationToken)
        => SendAsync(new DeactivateVolumePricingCommand(pricingId), cancellationToken);

    [HttpDelete("{pricingId:guid}")]
    public Task<IActionResult> DeletePricing(
        [FromRoute] Guid pricingId,
        CancellationToken cancellationToken)
        => SendAsync(new DeleteVolumePricingCommand(pricingId), cancellationToken);
}
