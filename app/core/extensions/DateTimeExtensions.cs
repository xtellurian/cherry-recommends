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
        public static DateTime FirstDayOfWeek(this DateTime dt, DayOfWeek day, bool toUtc = true)
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
    }
}