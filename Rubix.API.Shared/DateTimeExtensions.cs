using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>        
        /// Returns the range of dates of the specified weekday
        /// between the current System.DateTime object and another date.
        /// </summary>
        /// <param name="currentDate">The current date</param>
        /// <param name="anotherDate">The other date of the interval to search</param>
        /// <param name="dayOfWeek">The weekday to find</param>
        /// <returns>The collection of dates of the specified weekday within the period.
        /// </returns>
        public static IEnumerable<DateTime> Weekdays(this DateTime currentDate,
                      DateTime anotherDate, DayOfWeek dayOfWeek)
        {
            int sign = anotherDate.CompareTo(currentDate);
            return Enumerable.Range(0, sign * anotherDate.Subtract(currentDate).Days)
                .Select(delta => currentDate.AddDays(sign * (1 + delta)))
                .Where(date => date.DayOfWeek == dayOfWeek);
        }

        /// <summary>
        /// Returns the count of the specified weekday between
        /// the current System.DateTime object and another date.
        /// </summary>
        /// <param name="currentDate">The current date</param>
        /// <param name="anotherDate">The other date of the interval to search</param>
        /// <param name="dayOfWeek">The weekday to find</param>
        /// <returns>The count of the specified weekday within the period.</returns>
        public static int WeekdayCount(this DateTime currentDate,
                          DateTime anotherDate, DayOfWeek dayOfWeek)
        {
            return currentDate.Weekdays(anotherDate, dayOfWeek).Count();
        }
    }
}
