using System;

namespace PRN232_EbayClone.Application.Performance.Helpers;

/// <summary>
/// Helper for eBay's monthly seller level evaluation schedule (20th of each month).
/// </summary>
public static class EvaluationScheduleHelper
{
    private const int EvaluationDay = 20;

    /// <summary>
    /// Check if today is the monthly evaluation day (20th).
    /// </summary>
    public static bool IsEvaluationDay(DateTime dateUtc)
    {
        return dateUtc.Day == EvaluationDay;
    }

    /// <summary>
    /// Calculate next evaluation date from a given reference date.
    /// </summary>
    public static DateOnly CalculateNextEvaluationDate(DateTime referenceUtc)
    {
        var referenceDate = DateOnly.FromDateTime(referenceUtc.Date);

        if (referenceDate.Day >= EvaluationDay)
        {
            // Next evaluation is in the following month
            var nextMonth = referenceDate.AddMonths(1);
            return new DateOnly(nextMonth.Year, nextMonth.Month, EvaluationDay);
        }

        // Next evaluation is this month
        return new DateOnly(referenceDate.Year, referenceDate.Month, EvaluationDay);
    }

    /// <summary>
    /// Get the last evaluation date before a given reference date.
    /// </summary>
    public static DateOnly GetLastEvaluationDate(DateTime referenceUtc)
    {
        var referenceDate = DateOnly.FromDateTime(referenceUtc.Date);

        if (referenceDate.Day >= EvaluationDay)
        {
            // Last evaluation was this month
            return new DateOnly(referenceDate.Year, referenceDate.Month, EvaluationDay);
        }

        // Last evaluation was previous month
        var previousMonth = referenceDate.AddMonths(-1);
        return new DateOnly(previousMonth.Year, previousMonth.Month, EvaluationDay);
    }

    /// <summary>
    /// Check if seller needs re-evaluation (used for batch jobs).
    /// </summary>
    public static bool ShouldEvaluateSeller(DateTime lastEvaluatedUtc, DateTime nowUtc)
    {
        var lastEvaluated = DateOnly.FromDateTime(lastEvaluatedUtc.Date);
        var nextEvaluationDue = CalculateNextEvaluationDate(lastEvaluatedUtc);
        var today = DateOnly.FromDateTime(nowUtc.Date);

        return today >= nextEvaluationDue;
    }
}