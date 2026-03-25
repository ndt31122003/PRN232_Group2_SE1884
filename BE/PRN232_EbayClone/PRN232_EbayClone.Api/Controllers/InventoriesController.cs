using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Listings.Inventory.Commands.CommitStock;
using PRN232_EbayClone.Application.Listings.Inventory.Commands.InitializeInventory;
using PRN232_EbayClone.Application.Listings.Inventory.Commands.ReleaseStock;
using PRN232_EbayClone.Application.Listings.Inventory.Commands.ReserveStock;
using PRN232_EbayClone.Application.Listings.Inventory.Queries.GetInventoryByListingId;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/inventories")]
public sealed class InventoriesController(ISender sender) : ApiController(sender)
{
    [HttpPost("initialize")]
    public Task<IActionResult> Initialize([FromBody] InitializeInventoryCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("reserve")]
    public Task<IActionResult> Reserve([FromBody] ReserveStockCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("commit")]
    public Task<IActionResult> Commit([FromBody] CommitStockCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("release")]
    public Task<IActionResult> Release([FromBody] ReleaseStockCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("listing/{listingId:guid}")]
    public Task<IActionResult> GetByListingId(Guid listingId, CancellationToken cancellationToken)
        => SendAsync(new GetInventoryByListingIdQuery(listingId), cancellationToken);
}