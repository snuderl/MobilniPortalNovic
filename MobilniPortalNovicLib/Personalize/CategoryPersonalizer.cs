using System;
using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class CategoryPersonalizer : IPersonalize
    {
        public MobilniPortalNovicContext12 Context { get; set; }

        /// <summary>
        /// Percent of categories to include
        /// </summary>
        public float CategoryTreshold { get; set; }

        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
            CategoryTreshold = 70;
        }

        /// <summary>
        /// Get news for given user
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public IQueryable<NewsFile> GetNews(User u)
        {
            var clicks = Context.Clicks.Include("NewsFile").Where(x => x.UserId == u.UserId).ToList();
            var goodCategories = GetDesiredCategories(clicks, CategoryTreshold);

            return Context.NewsFiles.Where(x => goodCategories.Contains(x.CategoryId)).OrderByDescending(x => x.PubDate);
        }

        /// <summary>
        /// Take IQueryable of clicks and choose good categories based on number of clicks per category and treeshold
        /// </summary>
        /// <param name="clicks">Clicks</param>
        /// <param name="treshold">How much percent of clicks do you want to match</param>
        /// <returns></returns>
        public static HashSet<int> GetDesiredCategories(IEnumerable<ClickCounter> clicks, float treshold)
        {
            var count = CategoryHelpers.NumberOfCliksPerCategory(clicks.Select(x => x.NewsFile));
            var totalClicks = clicks.Count();
            var goodCategories = new HashSet<int>();
            float total = 0;
            var enumerator = count.OrderByDescending(x => x.Value).GetEnumerator();
            while (total < treshold / 100)
            {
                enumerator.MoveNext();
                total += ((float)enumerator.Current.Value) / totalClicks;
                goodCategories.Add(enumerator.Current.Key);
            }
            return goodCategories;
        }

        public static IQueryable<ClickCounter> FilterClicksByDate(IQueryable<ClickCounter> clicks, DateTime target)
        {
            var timeOffsetHours = 1;
            var upper = target.TimeOfDay.Add(new TimeSpan(timeOffsetHours, 0, 1));
            var lower = target.TimeOfDay.Add(new TimeSpan(timeOffsetHours,0,1));


            var clicksByHour = clicks.Where(x => x.ClickDate.TimeOfDay < upper && x.ClickDate.TimeOfDay > lower);

            var workDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            var weekendDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var targetDays = new List<DayOfWeek>{target.DayOfWeek};
            if (workDays.Contains(target.DayOfWeek))
            {
                int position = workDays.IndexOf(target.DayOfWeek);
                if (position > 0)
                {
                    targetDays.Add(workDays[position - 1]);
                }
                if (position - 1 < workDays.Count - 1)
                {
                    targetDays.Add(workDays[position + 1]);
                }
            }
            else
            {
                targetDays = weekendDays;
            }

            var clicksByDay = clicksByHour.ToList().Where(x => targetDays.Contains(x.ClickDate.DayOfWeek));
            return clicksByDay.AsQueryable();
        }
    }
}