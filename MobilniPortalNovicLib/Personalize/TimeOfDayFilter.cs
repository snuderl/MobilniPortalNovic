using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class TimeOfDayFilter : Filter
    {
        public int timeOffsetHours { get; set; }
        public DateTime Target { get; set; }

        public TimeOfDayFilter(DateTime target)
        {
            timeOffsetHours = 2;
            Target = target;
        }

        public IQueryable<ClickCounter> Filter(IQueryable<ClickCounter> clicks)
        {
            return FilterClickyByTimeOfDay(clicks, Target);
        }

        public String GetMessage()
        {
            return "Time of day filter: +-"+ timeOffsetHours+"h.";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clicks">List of clicks</param>
        /// <param name="target">DateTime when to look for clicks</param>
        /// <param name="timeOffsetHours">Hours around given target to look for clicks</param>
        /// <returns></returns>
        public IQueryable<ClickCounter> FilterClickyByTimeOfDay(IQueryable<ClickCounter> clicks, DateTime target)
        {
            var upper = target.TimeOfDay.Add(new TimeSpan(timeOffsetHours, 0, 1)).TotalMinutes;
            upper = Math.Min(DateTimeHelpers.MaxDayOfTime, upper);
            var lower = target.TimeOfDay.Subtract(new TimeSpan(timeOffsetHours, 0, 1)).TotalMinutes;
            lower = Math.Max(0, lower);

            var clicksByHour = clicks.Where(x => x.TimeOfDay < upper && x.TimeOfDay > lower);
            return clicksByHour;
        }
    }
}
