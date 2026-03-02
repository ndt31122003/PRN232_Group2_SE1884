using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IReportDownloadRepository : IRepository<ReportDownload, Guid>
{
    Task<(IReadOnlyList<ReportDownload> Items, int TotalCount)> GetDownloadsAsync(
        Guid userId,
        ReportDownloadFilterDto filter,
        CancellationToken cancellationToken = default);
}
