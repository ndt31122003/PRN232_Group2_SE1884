using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Domain.Reports.Entities;

namespace PRN232_EbayClone.Application.Reports.Schedules.Mappings;

public static class ReportScheduleMappingExtensions
{
    public static ReportScheduleDto ToDto(this ReportSchedule schedule)
    {
        return new ReportScheduleDto(
            schedule.Id,
            schedule.Source,
            schedule.Type,
            schedule.Frequency,
            schedule.CreatedAtUtc,
            schedule.LastRunAtUtc,
            schedule.NextRunAtUtc,
            schedule.EndDateUtc,
            schedule.IsActive,
            schedule.DeliveryEmail);
    }
}
