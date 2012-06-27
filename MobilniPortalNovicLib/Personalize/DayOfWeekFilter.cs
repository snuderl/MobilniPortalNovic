using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    class DayOfWeekFilter : Filter
    {
        public DateTime Target { get; set; }
        private String message;

        public DayOfWeekFilter(DateTime target)
        {
            Target = Target;
        }

        public IQueryable<ClickCounter> FilterClicksByDayOfWeek(IQueryable<ClickCounter> clicks, DateTime target)
        {
            var targetDays = new List<int>();
            if (DateTimeHelpers.WorkWeek.Contains(target.DayOfWeek))
            {
                int position = DateTimeHelpers.WorkWeek.IndexOf(target.DayOfWeek) + 1;
                targetDays.Add(position);
                if (position > 0)
                {
                    targetDays.Add(position - 1);
                }
                if (position - 1 < DateTimeHelpers.WorkWeek.Count - 1)
                {
                    targetDays.Add(position + 1);
                }
                message = "Day of week filter applied(work week)";
            }
            else
            {
                targetDays = new List<int> { 6, 7 };
                message="Day of week filter applied(weekend)";
            }

            var clicksByDay = clicks.Where(x => targetDays.Contains(x.DayOfWeek));
            return clicksByDay;
        }

        public IQueryable<ClickCounter> Filter(IQueryable<ClickCounter> clicks)
        {
            return FilterClicksByDayOfWeek(clicks, Target);
        }

        public string GetMessage()
        {
            return message;
        }
    }
}
