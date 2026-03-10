using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IReportScheduleRepository : IRepository<ReportSchedule, Guid>
{
    Task<(IReadOnlyList<ReportSchedule> Items, int TotalCount)> GetSchedulesAsync(
        Guid userId,
        ReportScheduleFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid userId,
        string source,
        string type,
        CancellationToken cancellationToken = default);
}
