using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ReportDownloadRepository : Repository<ReportDownload, Guid>, IReportDownloadRepository
{
    public ReportDownloadRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<ReportDownload?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.ReportDownloads
            .SingleOrDefaultAsync(download => download.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ReportDownload> Items, int TotalCount)> GetDownloadsAsync(
        Guid userId,
        ReportDownloadFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.ReportDownloads
            .AsNoTracking()
            .Where(download => download.UserId == new UserId(userId));

        if (!string.IsNullOrWhiteSpace(filter.Source))
        {
            var normalizedSource = filter.Source.Trim();
            query = query.Where(download => download.Source == normalizedSource);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(download => download.Status == filter.Status);
        }

        if (filter.FromUtc.HasValue)
        {
            query = query.Where(download => download.RequestedAtUtc >= filter.FromUtc.Value);
        }

        if (filter.ToUtc.HasValue)
        {
            query = query.Where(download => download.RequestedAtUtc <= filter.ToUtc.Value);
        }

        query = query.OrderByDescending(download => download.RequestedAtUtc);

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = filter.GetSanitizedPageNumber();
        var pageSize = filter.GetSanitizedPageSize();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
