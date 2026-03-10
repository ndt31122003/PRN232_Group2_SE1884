using PRN232_EbayClone.Domain.Reports.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Reports.Entities;

public class ReportSchedule : AggregateRoot<Guid>
{
    private const int SourceMaxLength = 64;
    private const int TypeMaxLength = 128;
    private const int EmailMaxLength = 256;

    public UserId UserId { get; private set; }
    public string Source { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public ReportScheduleFrequency Frequency { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastRunAtUtc { get; private set; }
    public DateTime? NextRunAtUtc { get; private set; }
    public DateTime? EndDateUtc { get; private set; }
    public bool IsActive { get; private set; }
    public string? DeliveryEmail { get; private set; }

    private ReportSchedule(Guid id) : base(id)
    {
    }

    public static Result<ReportSchedule> Create(
        UserId userId,
        string source,
        string type,
        ReportScheduleFrequency frequency,
        DateTime createdAtUtc,
        DateTime? endDateUtc = null,
        string? deliveryEmail = null)
    {
        if (userId.Value == Guid.Empty)
        {
            return Error.Failure("ReportSchedule.InvalidUser","UserId cannot be empty.");
        }

        var sanitizedSource = SanitizeRequiredLabel(source, SourceMaxLength, "ReportSchedule.InvalidSource", "Source is required.");
        if (sanitizedSource.IsFailure)
        {
            return sanitizedSource.Error;
        }

        var sanitizedType = SanitizeRequiredLabel(type, TypeMaxLength, "ReportSchedule.InvalidType", "Type is required.");
        if (sanitizedType.IsFailure)
        {
            return sanitizedType.Error;
        }

        if (endDateUtc.HasValue && endDateUtc.Value < createdAtUtc.Date)
        {
            return Error.Failure("ReportSchedule.InvalidEndDate","End date cannot be before the schedule is created.");
        }

        string? normalizedEmail = null;
        if (!string.IsNullOrWhiteSpace(deliveryEmail))
        {
            var trimmed = deliveryEmail.Trim();
            if (trimmed.Length > EmailMaxLength)
            {
                return Error.Failure("ReportSchedule.EmailTooLong",$"Delivery email must be {EmailMaxLength} characters or fewer.");
            }

            normalizedEmail = trimmed.ToLowerInvariant();
        }

        var schedule = new ReportSchedule(Guid.NewGuid())
        {
            UserId = userId,
            Source = sanitizedSource.Value,
            Type = sanitizedType.Value,
            Frequency = frequency,
            CreatedAtUtc = createdAtUtc,
            LastRunAtUtc = null,
            IsActive = true,
            EndDateUtc = endDateUtc,
            DeliveryEmail = normalizedEmail
        };

        schedule.NextRunAtUtc = schedule.CalculateNextRun(createdAtUtc);

        return Result.Success(schedule);
    }

    public Result UpdateFrequency(ReportScheduleFrequency frequency)
    {
        if (!IsActive)
        {
            return Error.Failure("ReportSchedule.Inactive","Cannot update an inactive schedule.");
        }

        if (Frequency == frequency)
        {
            return Result.Success();
        }

        Frequency = frequency;
        NextRunAtUtc = CalculateNextRun(DateTime.UtcNow);
        return Result.Success();
    }

    public Result SetEndDate(DateTime? endDateUtc)
    {
        if (endDateUtc.HasValue && endDateUtc.Value < CreatedAtUtc.Date)
        {
            return Error.Failure("ReportSchedule.InvalidEndDate","End date cannot be in the past relative to creation time.");
        }

        EndDateUtc = endDateUtc;
        return Result.Success();
    }

    public void MarkRunCompleted(DateTime executedAtUtc)
    {
        LastRunAtUtc = executedAtUtc;
        NextRunAtUtc = CalculateNextRun(executedAtUtc);
    }

    public void Deactivate(DateTime? endDateUtc = null)
    {
        IsActive = false;
        EndDateUtc = endDateUtc ?? EndDateUtc ?? DateTime.UtcNow;
    }

    private DateTime CalculateNextRun(DateTime fromUtc)
    {
        return Frequency switch
        {
            ReportScheduleFrequency.Hourly => fromUtc.AddHours(1),
            ReportScheduleFrequency.Daily => fromUtc.AddDays(1),
            ReportScheduleFrequency.Weekly => fromUtc.AddDays(7),
            ReportScheduleFrequency.Monthly => fromUtc.AddMonths(1),
            _ => fromUtc.AddDays(1)
        };
    }

    private static Result<string> SanitizeRequiredLabel(string value, int maxLength, string errorCode, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Failure(errorCode, errorMessage);
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            return Error.Failure(errorCode,$"Value must be {maxLength} characters or fewer.");
        }

        return Result.Success(trimmed);
    }
}
