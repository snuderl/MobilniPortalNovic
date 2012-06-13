using System;
using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace Worker
{
    public class FillDatabase
    {
        private MobilniPortalNovicContext12 context;

        public FillDatabase(MobilniPortalNovicContext12 context)
        {
            this.context = context;
        }

        public IEnumerable<ClickCounter> SimulateClicks(int userId, int Category, int count, Func<DateTime> dateTimeGenerator)
        {
            var list = new List<ClickCounter>();
            Random rnd = new Random();
            IEnumerable<int> indexes = new List<int>();
            var newsFiles = context.NewsFiles;
            var categories = context.Categories;
            var rows = CategoryHelpers.getRowsByCategory(newsFiles, Category, categories).OrderBy(x => new Guid()).Take(count).ToList();
            foreach (var r in rows)
            {
                var click = new ClickCounter { CategoryId = r.CategoryId, ClickDate = dateTimeGenerator(), NewsId = r.NewsId, UserId = userId, Location = "null" };
                click.SetDayOfWeekAndTimeOfDay();
                list.Add(context.Clicks.Add(click));
            }

            context.SaveChanges();
            return list;
        }
    }
}