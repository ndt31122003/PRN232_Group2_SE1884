using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SellerHub.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/seller-hub")]
[Authorize]
public sealed class SellerHubController : ApiController
{
    private readonly IUserContext _userContext;

    public SellerHubController(ISender sender, IUserContext userContext) : base(sender)
    {
        _userContext = userContext;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverviewAsync(CancellationToken cancellationToken)
    {
        var query = new GetOverviewSummaryQuery();
        var result = await SendAsync(query, cancellationToken);
        return result;
    }
}
