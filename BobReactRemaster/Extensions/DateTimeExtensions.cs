﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNextDateTimeFromTodayWithDayAndTime(this DateTime daytime,DayOfWeek day)
        {
            DateTime Date = DateTime.Today;
            Date.SetTime(daytime.Hour, daytime.Minute);
            int daysUntilDay = ((int)day - (int)Date.DayOfWeek + 7) % 7;
            return Date.AddDays(daysUntilDay);
        }
        public static DateTime GetNextDateTimeWithDayAndTime(this DateTime date,DateTime time, DayOfWeek day)
        {
            var tmp = date.SetTime(time.Hour, time.Minute);
            int daysUntilDay = ((int)day - (int)tmp.DayOfWeek + 7) % 7;
            return tmp.AddDays(daysUntilDay);
        }
        public static DateTime SetTime(this DateTime current, int hour)
        {
            return SetTime(current, hour, 0, 0, 0);
        }

        /// <summary>
        ///     Sets the time of the current date with minute precision.
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute)
        {
            return SetTime(current, hour, minute, 0, 0);
        }

        /// <summary>
        ///     Sets the time of the current date with second precision.
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute, int second)
        {
            return SetTime(current, hour, minute, second, 0);
        }

        /// <summary>
        ///     Sets the time of the current date with millisecond precision.
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(current.Year, current.Month, current.Day, hour, minute, second, millisecond);
        }
    }
}
