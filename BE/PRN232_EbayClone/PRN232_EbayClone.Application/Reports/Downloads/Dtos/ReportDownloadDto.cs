namespace PRN232_EbayClone.Application.Reports.Downloads.Dtos;

public sealed record ReportDownloadDto(
    Guid Id,
    string ReferenceCode,
    string Source,
    string Type,
    string Status,
    DateTime RequestedAtUtc,
    DateTime? CompletedAtUtc,
    string? FileUrl,
    ReportDownloadDateRangeDto? DateRange);

public sealed record ReportDownloadDateRangeDto(
    DateTime? StartUtc,
    DateTime? EndUtc,
    string? TimeZone);
