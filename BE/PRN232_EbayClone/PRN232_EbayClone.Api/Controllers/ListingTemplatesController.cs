using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.ListingTemplates.Commands;
using PRN232_EbayClone.Application.ListingTemplates.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/listing-templates")]
public class ListingTemplatesController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetTemplates([FromQuery] GetListingTemplatesQuery query, CancellationToken cancellationToken)
        => SendAsync(query, cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetTemplate(Guid id, CancellationToken cancellationToken)
        => SendAsync(new GetListingTemplateByIdQuery(id), cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateTemplate([FromBody] CreateListingTemplateCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPut("{id:guid}")]
    public Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateListingTemplateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateListingTemplateCommand(
            id,
            request.Name,
            request.Description,
            request.FormatLabel,
            request.ThumbnailUrl,
            request.Payload?.RootElement ?? default);

        return SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteTemplate(Guid id, CancellationToken cancellationToken)
        => SendAsync(new DeleteListingTemplateCommand(id), cancellationToken);

    [HttpPost("{id:guid}/clone")]
    public Task<IActionResult> CloneTemplate(Guid id, [FromBody] CloneListingTemplateRequest request, CancellationToken cancellationToken)
    {
        var command = new CloneListingTemplateCommand(id, request?.NameOverride);
        return SendAsync(command, cancellationToken);
    }
}

public sealed record UpdateListingTemplateRequest(
    string Name,
    string? Description,
    string? FormatLabel,
    string? ThumbnailUrl,
    JsonDocument? Payload
);

public sealed record CloneListingTemplateRequest(string? NameOverride);
