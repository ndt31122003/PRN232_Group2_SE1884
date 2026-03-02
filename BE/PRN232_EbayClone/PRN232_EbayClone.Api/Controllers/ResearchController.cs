using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Research.Commands;
using PRN232_EbayClone.Application.Research.Queries;
using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/research")]
[Authorize]
public sealed class ResearchController : ApiController
{
    private readonly IUserContext _userContext;

    public ResearchController(ISender sender, IUserContext userContext) : base(sender)
    {
        _userContext = userContext;
    }

    [HttpGet("product")]
    public async Task<IActionResult> GetProductResearchAsync(
        [FromQuery] string? keyword,
        [FromQuery] int? days,
        [FromQuery] string? format,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] bool? freeShippingOnly,
        [FromQuery] Guid? categoryId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var filters = BuildFilters(format, minPrice, maxPrice, freeShippingOnly, categoryId);

        var query = new GetProductResearchQuery(
            keyword,
            days ?? 30,
            page.GetValueOrDefault(1),
            pageSize.GetValueOrDefault(20),
            filters);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("sourcing")]
    public async Task<IActionResult> GetSourcingInsightsAsync(
        [FromQuery] string? keyword,
        [FromQuery] string? sort,
        [FromQuery] bool? savedOnly,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if (Guid.TryParse(_userContext.UserId, out var parsed))
        {
            userId = parsed;
        }

        var query = new GetSourcingInsightsQuery(
            keyword,
            sort,
            savedOnly ?? false,
            page.GetValueOrDefault(1),
            pageSize.GetValueOrDefault(12),
            userId);

        return await SendAsync(query, cancellationToken);
    }

    [HttpPost("sourcing/saved/{categoryId}")]
    public async Task<IActionResult> SaveSourcingCategoryAsync(
        [FromRoute] string categoryId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new SaveSourcingCategoryCommand(userId, categoryId);
        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("sourcing/saved/{categoryId}")]
    public async Task<IActionResult> RemoveSourcingCategoryAsync(
        [FromRoute] string categoryId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new RemoveSourcingCategoryCommand(userId, categoryId);
        return await SendAsync(command, cancellationToken);
    }

    private static ProductResearchFilters BuildFilters(
        string? format,
        decimal? minPrice,
        decimal? maxPrice,
        bool? freeShippingOnly,
        Guid? categoryId)
    {
        ListingFormat? listingFormat = null;

        if (!string.IsNullOrWhiteSpace(format))
        {
            var normalized = format.Trim();

            listingFormat = normalized.ToLowerInvariant() switch
            {
                "auction" => ListingFormat.Auction,
                "fixed" or "fixedprice" or "fixed_price" => ListingFormat.FixedPrice,
                _ when Enum.TryParse<ListingFormat>(normalized, true, out var parsed) => parsed,
                _ => null
            };
        }

        decimal? sanitizedMin = minPrice is > 0 ? minPrice : minPrice == 0 ? 0 : null;
        decimal? sanitizedMax = maxPrice is > 0 ? maxPrice : maxPrice == 0 ? 0 : null;

        if (sanitizedMin.HasValue && sanitizedMax.HasValue && sanitizedMin > sanitizedMax)
        {
            (sanitizedMin, sanitizedMax) = (sanitizedMax, sanitizedMin);
        }

        return new ProductResearchFilters(
            listingFormat,
            sanitizedMin,
            sanitizedMax,
            freeShippingOnly ?? false,
            categoryId);
    }
}
