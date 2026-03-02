using PRN232_EbayClone.Domain.Reports.Enums;

namespace PRN232_EbayClone.Application.Reports.Downloads.Dtos;

public sealed class ReportDownloadFilterDto
{
    private const int DefaultPageSize = 25;
    private const int MaxPageSize = 200;

    public string? Source { get; init; }
    public ReportDownloadStatus? Status { get; init; }
    public DateTime? FromUtc { get; init; }
    public DateTime? ToUtc { get; init; }
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
