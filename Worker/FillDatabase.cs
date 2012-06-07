using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace Worker
{

    public class FillDatabase
    {
        public void SimulateClicks(int userId, int Category, int count, Func<DateTime> dateTimeGenerator)
        {
            using (var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12())
            {
                Random rnd = new Random();
                IEnumerable<int> indexes = new List<int>();
                var news = context.NewsFiles.Where(x => x.CategoryId == Category).ToList();
                var rows = getRowsByCategory(context.NewsFiles, Category, context.Categories);
                rows = getNumberOfRandomRows(rows, count);
                foreach (var r in rows.ToList())
                {
                    var click = new ClickCounter { CategoryId = Category, ClickDate = dateTimeGenerator(), NewsId = r.NewsId, UserId = userId, Location = "null" };
                    context.Clicks.Add(click);
                }

                context.SaveChanges();

            }
        }

        public IQueryable<NewsFile> getRowsByCategory(IQueryable<NewsFile> query,int category, IQueryable<Category> categories)
        {
            var queryTmp = query;
            var parent = category;
            while (query.Count() == 0)
            {
                var childrenCategories = categories.Where(x => x.ParentCategoryId == parent).Select(x=>x.CategoryId).ToList();
                queryTmp = query.Where(x => childrenCategories.Contains(x.CategoryId));
            }

            return query;
        }

        public IQueryable<T> getNumberOfRandomRows<T>(IQueryable<T> query, int count)
        {


            return query.OrderBy(x=>Guid.NewGuid()).Take(count);
        }
    }
}
