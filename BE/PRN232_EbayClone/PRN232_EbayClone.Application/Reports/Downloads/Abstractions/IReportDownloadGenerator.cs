using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Domain.Reports.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Reports.Downloads.Abstractions;

public interface IReportDownloadGenerator
{
    Task<Result<GeneratedReportFile>> GenerateAsync(
        Guid userId,
        string source,
        string type,
        ReportDateRange? dateRange,
        CancellationToken cancellationToken = default);
}

public sealed record GeneratedReportFile(
    string FileName,
    string ContentType,
    byte[] ContentBytes);
