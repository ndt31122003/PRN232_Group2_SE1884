using System;

namespace PRN232_EbayClone.Application.Performance.Helpers;

/// <summary>
/// Helper for calculating business days (excluding weekends).
/// eBay uses business days for handling time calculations.
/// </summary>
public static class BusinessDaysHelper
{
    /// <summary>
    /// Add business days to a date (skipping Saturday and Sunday).
    /// </summary>
    /// <param name="startDate">Starting date (UTC)</param>
    /// <param name="businessDays">Number of business days to add</param>
    /// <returns>Date after adding business days (end of day)</returns>
    public static DateTime AddBusinessDays(DateTime startDate, int businessDays)
    {
        if (businessDays < 0)
        {
            throw new ArgumentException("Business days must be non-negative.", nameof(businessDays));
        }

        if (businessDays == 0)
        {
            return startDate;
        }

        var current = startDate;
        var daysAdded = 0;

        while (daysAdded < businessDays)
        {
            current = current.AddDays(1);

            // Skip weekends
            if (current.DayOfWeek != DayOfWeek.Saturday 
                && current.DayOfWeek != DayOfWeek.Sunday)
            {
                daysAdded++;
            }
        }

        // Return end of business day (11:59:59 PM)
        return new DateTime(
            current.Year, 
            current.Month, 
            current.Day, 
            23, 59, 59, 
            DateTimeKind.Utc);
    }

    /// <summary>
    /// Check if a date is a business day (Monday-Friday).
    /// </summary>
    public static bool IsBusinessDay(DateTime date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday 
            && date.DayOfWeek != DayOfWeek.Sunday;
    }

    /// <summary>
    /// Calculate number of business days between two dates.
    /// </summary>
    public static int CountBusinessDays(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date must be after start date.");
        }

        var businessDays = 0;
        var current = startDate.Date;
        var end = endDate.Date;

        while (current <= end)
        {
            if (IsBusinessDay(current))
            {
                businessDays++;
            }

            current = current.AddDays(1);
        }

        return businessDays;
    }
}