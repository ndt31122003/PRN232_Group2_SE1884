using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Reports.Downloads.Commands;
using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Application.Reports.Downloads.Queries;
using PRN232_EbayClone.Application.Reports.Schedules.Commands;
using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Application.Reports.Schedules.Queries;
using PRN232_EbayClone.Domain.Reports.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/reports")]
[Authorize]
public sealed class ReportsController : ApiController
{
    private readonly IUserContext _userContext;

    public ReportsController(ISender sender, IUserContext userContext) : base(sender)
    {
        _userContext = userContext;
    }

    [HttpGet("downloads")]
    public async Task<IActionResult> GetDownloadsAsync(
        [FromQuery] ReportDownloadFilterDto? filter,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetReportDownloadsQuery(userId, filter ?? new ReportDownloadFilterDto());
        return await SendAsync(query, cancellationToken);
    }

    [HttpPost("downloads")]
    public async Task<IActionResult> CreateDownloadAsync(
        [FromBody] CreateReportDownloadRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateReportDownloadCommand(
            userId,
            request.Source,
            request.Type,
            request.RangeStartUtc,
            request.RangeEndUtc,
            request.TimeZone);

        return await SendAsync(command, cancellationToken);
    }

    [HttpGet("schedules")]
    public async Task<IActionResult> GetSchedulesAsync(
        [FromQuery] ReportScheduleFilterDto? filter,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetReportSchedulesQuery(userId, filter ?? new ReportScheduleFilterDto());
        return await SendAsync(query, cancellationToken);
    }

    [HttpPost("schedules")]
    public async Task<IActionResult> CreateScheduleAsync(
        [FromBody] CreateReportScheduleRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateReportScheduleCommand(
            userId,
            request.Source,
            request.Type,
            request.Frequency,
            request.EndDateUtc,
            request.DeliveryEmail);

        return await SendAsync(command, cancellationToken);
    }

    public sealed record CreateReportDownloadRequest
    {
        public string Source { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public DateTime? RangeStartUtc { get; init; }
        public DateTime? RangeEndUtc { get; init; }
        public string? TimeZone { get; init; }
    }

    public sealed record CreateReportScheduleRequest
    {
        public string Source { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public ReportScheduleFrequency Frequency { get; init; }
        public DateTime? EndDateUtc { get; init; }
        public string? DeliveryEmail { get; init; }
    }
}
