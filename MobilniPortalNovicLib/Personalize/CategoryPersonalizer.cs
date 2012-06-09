using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class CategoryPersonalizer : IPersonalize
    {
        public MobilniPortalNovicContext12 Context { get; set; }

        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
        }

        public IQueryable<NewsFile> GetNews(User u)
        {
            var dict = CategoryHelpers.createCategoryParentLookup(Context.Categories.ToList());
            var count = Context.Clicks.Where(x => x.UserId == u.UserId).GroupBy(x => x.CategoryId).
                Select(x => new { Key = x.Key, count = x.Count() }).ToList();
            var categoryCount = new Dictionary<int, int>();
            foreach (var i in count)
            {
                var id = dict[i.Key];
                if (!categoryCount.ContainsKey(id))
                {
                    categoryCount[id] = i.count;
                }
                else
                {
                    categoryCount[id] += i.count;
                }
            }

            var max = categoryCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            var ids = dict.Where(x => x.Value == max).Select(x => x.Key);

            return Context.NewsFiles.Where(x => ids.Contains(x.CategoryId));
        }
    }
}