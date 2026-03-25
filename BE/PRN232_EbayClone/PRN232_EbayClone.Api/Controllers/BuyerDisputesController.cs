using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/buyer/disputes")]
public sealed class BuyerDisputesController(ISender sender, ICurrentUser currentUser) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetBuyerDisputes(
        [FromQuery] DisputeFilterDto filter,
        CancellationToken cancellationToken)
    {
        var buyerId = currentUser.UserId;
        if (string.IsNullOrEmpty(buyerId))
        {
            return Unauthorized();
        }

        Console.WriteLine($"[BuyerDisputesController] GetBuyerDisputes called by buyer: {buyerId}");
        Console.WriteLine($"[BuyerDisputesController] Filter: Status={filter.Status}, PageNumber={filter.PageNumber}, PageSize={filter.PageSize}");

        var query = new GetBuyerDisputesQuery(buyerId, filter);
        return await SendAsync(query, cancellationToken);
    }
}
