using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var dict = createCategoryParentLookup();
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
            var ids = dict.Where(x=>x.Value==max).Select(x=>x.Key);

            return Context.NewsFiles.Where(x=>ids.Contains(x.CategoryId));
        }

        public Dictionary<int, int> createCategoryParentLookup()
        {
            var dict = new Dictionary<int, int>();
            var categories = Context.Categories.ToList();
            foreach (var i in categories)
            {
                if (i.ParentCategoryId == null)
                {
                    dict.Add(i.CategoryId, i.CategoryId);
                }
                else
                {
                    var c = i;
                    while (c.ParentCategoryId != null)
                    {
                        c = c.ParentCategory;
                    }
                    dict.Add(i.CategoryId, c.CategoryId);
                }
            }
            return dict;
        }
    }
}
