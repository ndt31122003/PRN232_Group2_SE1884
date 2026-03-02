using PRN232_EbayClone.Domain.Reports.Enums;

namespace PRN232_EbayClone.Application.Reports.Schedules.Dtos;

public sealed record ReportScheduleDto(
    Guid Id,
    string Source,
    string Type,
    ReportScheduleFrequency Frequency,
    DateTime CreatedAtUtc,
    DateTime? LastRunAtUtc,
    DateTime? NextRunAtUtc,
    DateTime? EndDateUtc,
    bool IsActive,
    string? DeliveryEmail);
