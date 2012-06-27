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
        public List<String> Messages { get; private set; }
        public int MinimalClicks { get; set; }
        public int HourOffset { get; set; }
        public List<Category> GoodCategories { get; set; }

        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
            CategoryTreshold = 70;
            Messages = new List<String>();
            HourOffset = 2;
            MinimalClicks = 10;
        }

        /// <summary>
        /// Get news for given user
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public IQueryable<NewsFile> GetNews(User u)
        {
            var clicks = Context.Clicks.Include("NewsFile").Where(x => x.UserId == u.UserId);
            int count = clicks.Count();
            if (count < MinimalClicks)
            {
                return Context.NewsFiles.OrderByDescending(x => x.PubDate);
            }

            var filteredByTimeOfDay = FilterClickyByTimeOfDay(clicks, DateTime.Now, HourOffset);
            if (filteredByTimeOfDay.Count() > MinimalClicks)
            {
                Messages.Add("Time of day filter applied");
            }
            else
            {
                filteredByTimeOfDay = clicks;
            }

            var filteredByDayOfWeek = FilterClicksByDayOfWeek(filteredByTimeOfDay, DateTime.Now);
            if (filteredByDayOfWeek.Count() > MinimalClicks)
            {
                Messages.Add("Day of week filter applied");
            }
            else
            {
                filteredByDayOfWeek = clicks;
            }

            var goodCategories = GetDesiredCategories(filteredByDayOfWeek.ToList(), CategoryTreshold);
            GoodCategories = Context.Categories.Include("ParentCategory").Where(x => goodCategories.Contains(x.CategoryId)).ToList();


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clicks">List of clicks</param>
        /// <param name="target">DateTime when to look for clicks</param>
        /// <param name="timeOffsetHours">Hours around given target to look for clicks</param>
        /// <returns></returns>
        public static IQueryable<ClickCounter> FilterClickyByTimeOfDay(IQueryable<ClickCounter> clicks, DateTime target, int timeOffsetHours = 1)
        {
            var upper = target.TimeOfDay.Add(new TimeSpan(timeOffsetHours, 0, 1)).TotalMinutes;
            upper = Math.Min(DateTimeHelpers.MaxDayOfTime, upper);
            var lower = target.TimeOfDay.Subtract(new TimeSpan(timeOffsetHours, 0, 1)).TotalMinutes;
            lower = Math.Max(0, lower);

            var clicksByHour = clicks.Where(x => x.TimeOfDay < upper && x.TimeOfDay > lower);
            return clicksByHour;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clicks">Click queryable</param>
        /// <param name="N">Number of nearest clicks</param>
        /// <param name="GivenPosition">Position to look fod</param>
        /// <returns></returns>
        public static IQueryable<ClickCounter> FilterByNNearestClicks(IQueryable<ClickCounter> clicks, int N, Coordinates GivenPosition)
        {
            var l = clicks.Where(x=>x.Latitude!=null && x.Longitude!=null).ToList();
            var closest = l.OrderBy(x => CoordinateHelper.DistanceInM(x.Coordinates, GivenPosition)).Take(N);
            return closest.AsQueryable();            
        }

        public static IQueryable<ClickCounter> FilterByClicksInGivenRadius(IQueryable<ClickCounter> clicks, double radiusInKm, Coordinates GivenPosition)
        {
            var l = clicks.Where(x => x.Latitude != null && x.Longitude != null).ToList();
            var closest = l.Where(x => CoordinateHelper.DistanceInM(x.Coordinates, GivenPosition) < radiusInKm/1000);
            return closest.AsQueryable();
        }

        public static IQueryable<ClickCounter> FilterClicksByDayOfWeek(IQueryable<ClickCounter> clicks, DateTime target)
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
            }
            else
            {
                targetDays = new List<int> { 6, 7 };
            }

            var clicksByDay = clicks.Where(x => targetDays.Contains(x.DayOfWeek));
            return clicksByDay;
        }
    }
}