using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Performance.Queries;
using Microsoft.Extensions.Logging;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/performance")]
public sealed class PerformanceController : ApiController
{
    private readonly IUserContext _userContext;
    private readonly ILogger<PerformanceController> _logger;

    public PerformanceController(ISender sender, IUserContext userContext, ILogger<PerformanceController> logger) : base(sender)
    {
        _userContext = userContext;
        _logger = logger;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync([FromQuery] string? period, CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        var query = new GetPerformanceSummaryQuery(sellerId, period);

		return await SendAsync(query, cancellationToken);
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReportAsync([FromQuery] string? period, [FromQuery] string? compare, CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        var query = new GetPerformanceSalesReportQuery(sellerId, period, compare);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("traffic")]
    public async Task<IActionResult> GetTrafficAsync([FromQuery] string? period, CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        var query = new GetPerformanceTrafficQuery(sellerId, period);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("seller-level")]
    public async Task<IActionResult> GetSellerLevelAsync(CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        try
        {
            var query = new GetPerformanceSellerLevelDetailsQuery(sellerId);
            return await SendAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch performance seller level for seller {SellerId}", sellerId);
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Seller level error", detail: ex.Message);
        }
    }

    [HttpGet("service-metrics")]
    public async Task<IActionResult> GetServiceMetricsAsync([FromQuery] string? period, CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        try
        {
            var query = new GetPerformanceServiceMetricsQuery(sellerId, period);
            return await SendAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch service metrics for seller {SellerId} with period {Period}", sellerId, period);
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Service metrics error", detail: ex.Message);
        }
    }

    [HttpGet("inventory-dashboard")]
    public async Task<IActionResult> GetInventoryDashboardAsync(CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        var query = new GetPerformanceInventoryDashboardQuery(sellerId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("seller-standards-warning")]
    public async Task<IActionResult> GetSellerStandardsWarningAsync(CancellationToken cancellationToken)
    {
        if (!TryGetSellerId(out var sellerId))
        {
            return Unauthorized();
        }

        var query = new CheckSellerStandardsWarningQuery(sellerId);
    
        var result = await SendAsync(query, cancellationToken);
    
        // If no warnings, return 204 No Content
        if (result is OkObjectResult okResult && okResult.Value is null)
        {
            return NoContent();
        }
    
        return result;
    }

    private bool TryGetSellerId(out Guid sellerId)
    {
        sellerId = default;
        return Guid.TryParse(_userContext.UserId, out sellerId);
    }
}