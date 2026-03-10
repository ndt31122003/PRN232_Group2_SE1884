using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Orders.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/shipping-services")]
public sealed class ShippingServicesController : ApiController
{
    public ShippingServicesController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetShippingServices(CancellationToken cancellationToken)
    {
        var query = new GetShippingServicesQuery();
        var result = await SendAsync(query, cancellationToken);
        return result;
    }
}
