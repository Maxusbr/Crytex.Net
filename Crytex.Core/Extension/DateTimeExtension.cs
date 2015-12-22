using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Core.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime TrimDate(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks);
        }

        public static DateTime TrimToGraterHour(this DateTime date)
        {
            var trimmedDate = new DateTime(
                date.Ticks % TimeSpan.TicksPerHour != 0 ? date.AddHours(1).Ticks - date.Ticks % TimeSpan.TicksPerHour : date.Ticks);

            return trimmedDate;
        }
    }
}
