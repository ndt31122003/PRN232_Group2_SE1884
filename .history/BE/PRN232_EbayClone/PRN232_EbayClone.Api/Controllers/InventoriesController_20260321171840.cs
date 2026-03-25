using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Listings.Inventory.Commands.InitializeInventory;
using PRN232_EbayClone.Application.Listings.Inventory.Queries.GetInventoryByListingId;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/inventories")]
public sealed class InventoriesController(ISender sender) : ApiController(sender)
{
    [HttpPost("initialize")]
    public Task<IActionResult> Initialize([FromBody] InitializeInventoryCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("listing/{listingId:guid}")]
    public Task<IActionResult> GetByListingId(Guid listingId, CancellationToken cancellationToken)
        => SendAsync(new GetInventoryByListingIdQuery(listingId), cancellationToken);
}