using PRN232_EbayClone.Domain.Reports.Enums;

namespace PRN232_EbayClone.Application.Reports.Schedules.Dtos;

public sealed class ReportScheduleFilterDto
{
    private const int DefaultPageSize = 25;
    private const int MaxPageSize = 200;

    public string? Source { get; init; }
    public string? Type { get; init; }
    public ReportScheduleFrequency? Frequency { get; init; }
    public bool OnlyActive { get; init; } = true;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = DefaultPageSize;

    public int GetSanitizedPageSize()
        => PageSize switch
        {
            <= 0 => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _ => PageSize
        };

    public int GetSanitizedPageNumber()
        => PageNumber <= 0 ? 1 : PageNumber;
}
