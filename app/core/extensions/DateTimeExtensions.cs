using System;

namespace SignalBox.Core
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset TruncateToYearStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
        public static DateTimeOffset TruncateToMonthStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        public static DateTimeOffset TruncateToDayStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        public static DateTimeOffset TruncateToHourStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }
        public static DateTimeOffset TruncateToMinuteStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }
        public static DateTimeOffset TruncateToSecondStart(this DateTimeOffset dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
        public static DateTimeOffset FirstDayOfWeek(this DateTimeOffset dt, DayOfWeek day, bool toUtc = true)
        {
            if (toUtc)
            {
                return dt.ToUniversalTime().AddDays(day - dt.DayOfWeek).Date;
            }
            else
            {
                return dt.AddDays(day - dt.DayOfWeek).Date;
            }
        }

        public static DateTimeOffset DateTimeSince(this DateTimeOffset dt, MetricGeneratorTimeWindow timeWindow)
        {
            switch (timeWindow)
            {
                case MetricGeneratorTimeWindow.SevenDays:
                    return dt.AddDays(-7);
                case MetricGeneratorTimeWindow.ThirtyDays:
                    return dt.AddDays(-30);
                default:
                    return DateTimeOffset.MinValue;
            }
        }

        /// <summary>
        /// Get the date time offset based on the specified period.
        /// <param name="period"></param>
        /// <param name="periodAgo">periodAgo of 0 refers to current period</param>
        /// </summary>
        public static DateTimeOffset DateTimeSince(this DateTimeOffset dt, DateTimePeriod period, int periodAgo)
        {
            switch (period)
            {
                case DateTimePeriod.Daily:
                    return dt.AddDays(-periodAgo);
                case DateTimePeriod.Weekly:
                    return dt.AddDays(-7 * periodAgo);
                case DateTimePeriod.Monthly:
                    return dt.AddMonths(-1 * periodAgo);
                default:
                    return dt;
            }
        }
    }
}