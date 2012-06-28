using System;
using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class NewsRequest
    {
        public DateTime TargetTime { get; set; }
        public Coordinates Location { get; set; }
        public User User { get; set; }
        public int RadiusInKm { get; set; }

        private NewsRequest(User u, Coordinates l)
        {
            TargetTime = DateTime.Now;
            Location = l;
            RadiusInKm = 10;
            User = u;
        }

        public static NewsRequest Construct(int id, MobilniPortalNovicContext12 context, Coordinates l = null)
        {
            var user = context.Users.Find(id);
            return new NewsRequest(user, l);
        }

        public static NewsRequest Construct(Guid token, MobilniPortalNovicContext12 context, Coordinates l = null)
        {
            var user = context.Users.Where(x => x.AccessToken == token).First();
            return new NewsRequest(user, l);
        }

        public static NewsRequest Construct(String username, MobilniPortalNovicContext12 context, Coordinates l = null)
        {
            var user = context.Users.Where(x => x.Username == username).First();
            return new NewsRequest(user, l);
        }
    }


    public class CategoryPersonalizer : IPersonalize
    {
        public MobilniPortalNovicContext12 Context { get; set; }

        public List<String> Messages { get; private set; }
        public List<Category> GoodCategories { get; set; }
        public List<Filter> Filters { get; set; }


        public int MinimalClicks { get; set; }
        public float CategoryTreshold { get; set; }

        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
            CategoryTreshold = 70;
            Messages = new List<String>();
            GoodCategories = new List<Category>();
            MinimalClicks = 10;
            Filters = new List<Filter>();
        }

        /// <summary>
        /// Get news for given user
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public IQueryable<NewsFile> GetNews(NewsRequest nr)
        {
            Messages.Clear();
            GoodCategories.Clear();
            Filters.Clear();

            if (nr.Location != null)
            {
                Filters.Add(new RadiusFilter(nr.RadiusInKm, nr.Location));
            }
            if (nr.TargetTime != null)
            {
                Filters.Add(new TimeOfDayFilter(nr.TargetTime));
                Filters.Add(new DayOfWeekFilter(nr.TargetTime));
            }


            var clicks = Context.Clicks.Include("NewsFile").Where(x => x.UserId == nr.User.UserId);
            foreach (Filter f in Filters)
            {
                var tmp = f.Filter(clicks);
                var count = tmp.Count();
                if (count > MinimalClicks)
                {
                    clicks = tmp;
                    Messages.Add(f.GetMessage());
                }
            }

            var goodCategories = GetDesiredCategories(clicks.ToList(), CategoryTreshold);
            if (goodCategories.Count == 0)
            {
                GoodCategories = Context.Categories.Include("ParentCategory").ToList();
                goodCategories = new HashSet<int>(GoodCategories.Select(x => x.CategoryId));
            }
            else
            {
                GoodCategories = Context.Categories.Include("ParentCategory").Where(x => goodCategories.Contains(x.CategoryId)).ToList();
            }


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
            var count = CategoryHelpers.NumberOfCliksPerCategory(clicks.Select(x => x.NewsFile)).OrderByDescending(x=>x.Value);
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
            goodCategories.Remove(0);
            return goodCategories;
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
            var l = clicks.Where(x => x.Latitude != null && x.Longitude != null).ToList();
            var closest = l.OrderBy(x => CoordinateHelper.DistanceInM(x.Coordinates, GivenPosition)).Take(N);
            return closest.AsQueryable();
        }


        public void AddMessage(string s)
        {
            throw new NotImplementedException();
        }
    }
}