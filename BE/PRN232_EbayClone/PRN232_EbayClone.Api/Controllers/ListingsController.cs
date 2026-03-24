using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Application.Listings.Common;
using PRN232_EbayClone.Application.Listings.Dtos;
using PRN232_EbayClone.Application.Listings.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/listings")]
public class ListingsController(ISender sender) : ApiController(sender)
{
    [HttpGet("active")]
    public Task<IActionResult> GetActiveListings([FromQuery] GetActiveListingsQuery query, CancellationToken cancellationToken)
        => SendAsync(query, cancellationToken);

    [HttpGet("scheduled")]
    public Task<IActionResult> GetScheduledListings([FromQuery] GetScheduledListingsQuery query, CancellationToken cancellationToken)
        => SendAsync(query, cancellationToken);

    [HttpGet("drafts")]
    public Task<IActionResult> GetDraftListings([FromQuery] GetDraftsQuery query, CancellationToken cancellationToken)
        => SendAsync(query, cancellationToken);

    [HttpGet("ended")]
    public Task<IActionResult> GetEndedListings([FromQuery] GetEndedListingsQuery query, CancellationToken cancellationToken)
        => SendAsync(query, cancellationToken);

    [HttpPost("drafts/bulk-delete")]
    public Task<IActionResult> BulkDeleteDrafts([FromBody] BulkDeleteDraftListingsCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("drafts/copy")]
    public Task<IActionResult> BulkCopyDrafts([FromBody] BulkCopyDraftListingsCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("relist")]
    public Task<IActionResult> RelistListings([FromBody] RelistListingsCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("sell-similar")]
    public Task<IActionResult> SellSimilarListings([FromBody] SellSimilarListingsCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("end")]
    public Task<IActionResult> EndListings([FromBody] BulkListingActionRequest request, CancellationToken cancellationToken)
    {
        var command = new EndListingsCommand(request.ListingIds?.ToList() ?? new List<Guid>());
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("send-to-drafts")]
    public Task<IActionResult> SendListingsToDrafts([FromBody] BulkListingActionRequest request, CancellationToken cancellationToken)
    {
        var command = new SendListingsToDraftsCommand(request.ListingIds?.ToList() ?? new List<Guid>());
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("send-offers")]
    public Task<IActionResult> SendOffers([FromBody] BulkListingActionRequest request, CancellationToken cancellationToken)
    {
        var command = new SendOffersCommand(request.ListingIds?.ToList() ?? new List<Guid>());
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("promote")]
    public Task<IActionResult> PromoteListings([FromBody] BulkListingActionRequest request, CancellationToken cancellationToken)
    {
        var command = new PromoteListingsCommand(request.ListingIds?.ToList() ?? new List<Guid>());
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("archive")]
    public Task<IActionResult> ArchiveListings([FromBody] BulkListingActionRequest request, CancellationToken cancellationToken)
    {
        var command = new ArchiveListingsCommand(request.ListingIds?.ToList() ?? new List<Guid>());
        return SendAsync(command, cancellationToken);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportListings([FromQuery] ExportListingsRequest request, CancellationToken cancellationToken)
    {
        var query = new ExportListingsQuery(
            request.Status,
            request.SearchTerm,
            request.ListingIds,
            request.MaxRows);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        var csv = ListingCsvFormatter.BuildExportCsv(result.Value);
        var fileName = $"listings-export-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
        var bytes = Encoding.UTF8.GetBytes(csv);

        return File(bytes, "text/csv", fileName);
    }

    [HttpPost("import")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> ImportListings(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "File upload error",
                Detail = "File rỗng hoặc không tồn tại.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var content = await reader.ReadToEndAsync(cancellationToken);

        var (rows, parseFailures) = ListingCsvFormatter.ParseImportCsv(content);

        if (rows.Count == 0)
        {
            var response = new ImportListingsCsvResult(0, parseFailures);
            return Ok(response);
        }

        var command = new ImportListingsCsvCommand(rows);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        var combinedFailures = result.Value.Failures.Concat(parseFailures).ToList();
        var payload = new ImportListingsCsvResult(result.Value.UpdatedCount, combinedFailures);

        return Ok(payload);
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetListing(Guid id, CancellationToken cancellationToken)
        => SendAsync(new GetListingByIdQuery(id), cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateListing(CreateListingCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPut]
    public Task<IActionResult> UpdateListing(UpdateListingCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteListing(Guid id, CancellationToken cancellationToken)
        => SendAsync(new RemoveListingCommand(id), cancellationToken);

    [HttpGet("offers")]
    public Task<IActionResult> GetListingOffers([FromQuery] Guid? listingId, CancellationToken cancellationToken)
        => SendAsync(new GetListingOffersQuery(listingId), cancellationToken);

    [HttpPatch("{id:guid}/offers/accept")]
    public Task<IActionResult> AcceptOffer(Guid id, [FromBody] AcceptOfferRequest request, CancellationToken cancellationToken)
    {
        var command = new AcceptOfferCommand(id, request.OfferAmount, request.BuyerId);
        return SendAsync(command, cancellationToken);
    }

    [HttpGet("bids")]
    public Task<IActionResult> GetListingBids([FromQuery] Guid? listingId, CancellationToken cancellationToken)
        => SendAsync(new GetListingBidsQuery(listingId), cancellationToken);

}

public sealed record AcceptOfferRequest(decimal OfferAmount, Guid BuyerId);

public sealed record ExportListingsRequest(
    string? Status,
    string? SearchTerm,
    Guid[]? ListingIds,
    int MaxRows = 2000
);

public sealed record BulkListingActionRequest(Guid[]? ListingIds);

