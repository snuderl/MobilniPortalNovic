using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilniPortalNovicLib.Helpers
{
    static public class DateTimeHelpers
    {

        public static List<DayOfWeek> weekDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
        public static List<DayOfWeek> WorkWeek { get { return weekDays.Take(5).ToList(); } }
        public static List<DayOfWeek> WeekEnd { get { return weekDays.Skip(5).Take(2).ToList(); } }

        static public int MaxDayOfTime { get { return 24 * 60; } }

    }
}
