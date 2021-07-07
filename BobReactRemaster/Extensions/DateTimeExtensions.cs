using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNextDateTimeWithDayAndTime(this DateTime daytime,DayOfWeek day)
        {
            DateTime Date = DateTime.Today;
            Date.AddHours(daytime.Hour);
            Date.AddMinutes(daytime.Minute);
            int daysUntilDay = ((int)day - (int)Date.DayOfWeek + 7) % 7;
            return Date.AddDays(daysUntilDay);
        }
    }
}
