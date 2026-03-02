using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.SaleEvents.Commands;
using PRN232_EbayClone.Application.SaleEvents.Queries;
using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/sale-events")]
public sealed class SaleEventsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetSellerSaleEvents(CancellationToken cancellationToken)
        => SendAsync(new GetSellerSaleEventsQuery(), cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        => SendAsync(new GetSaleEventByIdQuery(id), cancellationToken);

    [HttpGet("eligible-listings")]
    public Task<IActionResult> GetEligibleListings([FromQuery] GetSaleEventEligibleListingsRequest request, CancellationToken cancellationToken)
        => SendAsync(request.ToQuery(), cancellationToken);

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateSaleEventCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSaleEventRequest request, CancellationToken cancellationToken)
        => SendAsync(request.ToCommand(id), cancellationToken);

    [HttpPatch("{id:guid}/status")]
    public Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateSaleEventStatusRequest request, CancellationToken cancellationToken)
        => SendAsync(new UpdateSaleEventStatusCommand(id, request.Status), cancellationToken);

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        => SendAsync(new DeleteSaleEventCommand(id), cancellationToken);
}

public sealed record UpdateSaleEventRequest(
    string Name,
    string? Description,
    SaleEventMode Mode,
    DateTime StartDate,
    DateTime EndDate,
    bool OfferFreeShipping,
    bool IncludeSkippedItems,
    bool BlockPriceIncreaseRevisions,
    decimal? HighlightPercentage,
    IReadOnlyList<CreateSaleEventTierRequest>? Tiers)
{
    public UpdateSaleEventCommand ToCommand(Guid saleEventId) => new(
        saleEventId,
        Name,
        Description,
        Mode,
        StartDate,
        EndDate,
        OfferFreeShipping,
        IncludeSkippedItems,
        BlockPriceIncreaseRevisions,
        HighlightPercentage,
        Tiers);
}

public sealed record UpdateSaleEventStatusRequest(SaleEventStatus Status);

public sealed record GetSaleEventEligibleListingsRequest(
    string? SearchTerm,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinDaysOnSite,
    bool ExcludeAlreadyAssigned = true,
    int PageNumber = 1,
    int PageSize = 25)
{
    public GetSaleEventEligibleListingsQuery ToQuery() => new(
        SearchTerm,
        CategoryId,
        MinPrice,
        MaxPrice,
        MinDaysOnSite,
        ExcludeAlreadyAssigned,
        PageNumber,
        PageSize);
}
