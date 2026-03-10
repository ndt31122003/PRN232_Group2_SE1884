using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;

namespace PRN232_EbayClone.Application.Reports.Downloads.Mappings;

public static class ReportDownloadMappingExtensions
{
    public static ReportDownloadDto ToDto(this ReportDownload download)
    {
        return new ReportDownloadDto(
            download.Id,
            download.ReferenceCode,
            download.Source,
            download.Type,
            download.Status.ToString(),
            download.RequestedAtUtc,
            download.CompletedAtUtc,
            download.FileUrl,
            download.DateRange is null
                ? null
                : new ReportDownloadDateRangeDto(
                    download.DateRange.StartUtc,
                    download.DateRange.EndUtc,
                    download.DateRange.TimeZone));
    }
}
