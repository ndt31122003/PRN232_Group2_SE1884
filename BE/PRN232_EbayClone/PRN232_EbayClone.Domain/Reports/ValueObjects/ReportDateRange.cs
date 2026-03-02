using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Reports.ValueObjects;

public sealed record ReportDateRange(DateTime? StartUtc, DateTime? EndUtc, string? TimeZone)
{
    public static Result<ReportDateRange> Create(DateTime? startUtc, DateTime? endUtc, string? timeZone)
    {
        if (startUtc.HasValue && endUtc.HasValue && startUtc.Value > endUtc.Value)
        {
            return Error.Failure("ReportDateRange.Invalid","Start date must be before or equal to end date.");
        }

        string? normalizedTimeZone = string.IsNullOrWhiteSpace(timeZone)
            ? null
            : timeZone.Trim();

        if (normalizedTimeZone is { Length: > 64 })
        {
            return Error.Failure("ReportDateRange.TimeZoneTooLong","Time zone label must be 64 characters or fewer.");
        }

        return Result.Success(new ReportDateRange(startUtc, endUtc, normalizedTimeZone));
    }
}
