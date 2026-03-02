using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Stores.Commands;
using PRN232_EbayClone.Application.Stores.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/stores")]
public sealed class StoresController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public Task<IActionResult> CreateStore([FromBody] CreateStoreCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("{storeId}")]
    public Task<IActionResult> GetStoreById(string storeId, CancellationToken cancellationToken)
        => SendAsync(new GetStoreByIdQuery(storeId), cancellationToken);

    [HttpGet("my-stores")]
    public Task<IActionResult> GetMyStores(CancellationToken cancellationToken)
        => SendAsync(new GetUserStoresQuery(), cancellationToken);

    [HttpPut("{storeId}")]
    public Task<IActionResult> UpdateStoreProfile(string storeId, [FromBody] UpdateStoreProfileRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStoreProfileCommand(
            storeId,
            request.Name,
            request.Description,
            request.LogoUrl,
            request.BannerUrl);
        return SendAsync(command, cancellationToken);
    }
}

public sealed record UpdateStoreProfileRequest(
    string Name,
    string? Description = null,
    string? LogoUrl = null,
    string? BannerUrl = null
);
