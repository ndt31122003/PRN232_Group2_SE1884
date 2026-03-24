using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Application.Listings.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/listings")]
[ApiController]
public class PublicListingsController : ControllerBase
{
    private readonly ISender _sender;

    public PublicListingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}/public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicListing(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPublicListingByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("{id}/offers")]
    [Authorize]
    public async Task<IActionResult> CreateOffer(Guid id, [FromBody] CreateOfferRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOfferCommand(id, request.Amount);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{id}/bids")]
    [Authorize]
    public async Task<IActionResult> PlaceBid(Guid id, [FromBody] PlaceBidRequest request, CancellationToken cancellationToken)
    {
        var command = new PlaceBidCommand(id, request.Amount);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

public record CreateOfferRequest(decimal Amount);
public record PlaceBidRequest(decimal Amount);
