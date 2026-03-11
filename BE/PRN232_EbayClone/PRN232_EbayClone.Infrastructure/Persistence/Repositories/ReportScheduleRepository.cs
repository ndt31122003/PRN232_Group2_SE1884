using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ReportScheduleRepository : Repository<ReportSchedule, Guid>, IReportScheduleRepository
{
    public ReportScheduleRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<ReportSchedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.ReportSchedules
            .SingleOrDefaultAsync(schedule => schedule.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<ReportSchedule> Items, int TotalCount)> GetSchedulesAsync(
        Guid userId,
        ReportScheduleFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.ReportSchedules
            .AsNoTracking()
            .Where(schedule => schedule.UserId == new UserId(userId));

        if (!string.IsNullOrWhiteSpace(filter.Source))
        {
            var normalizedSource = filter.Source.Trim();
            query = query.Where(schedule => schedule.Source == normalizedSource);
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            var normalizedType = filter.Type.Trim();
            query = query.Where(schedule => schedule.Type == normalizedType);
        }

        if (filter.Frequency.HasValue)
        {
            query = query.Where(schedule => schedule.Frequency == filter.Frequency);
        }

        if (filter.OnlyActive)
        {
            query = query.Where(schedule => schedule.IsActive);
        }

        query = query.OrderByDescending(schedule => schedule.CreatedAtUtc);

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = filter.GetSanitizedPageNumber();
        var pageSize = filter.GetSanitizedPageSize();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> ExistsAsync(
        Guid userId,
        string source,
        string type,
        CancellationToken cancellationToken = default)
    {
        var normalizedSource = source.Trim();
        var normalizedType = type.Trim();

        return DbContext.ReportSchedules
            .AsNoTracking()
            .AnyAsync(schedule =>
                schedule.UserId.Value == userId &&
                schedule.IsActive &&
                schedule.Source == normalizedSource &&
                schedule.Type == normalizedType,
                cancellationToken);
    }
}
