using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Helpers;

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
                var rows = CategoryHelpers.getRowsByCategory(context.NewsFiles, Category, context.Categories);
                rows = CategoryHelpers.getNumberOfRandomRows(rows, count);
                foreach (var r in rows.ToList())
                {
                    var click = new ClickCounter { CategoryId = Category, ClickDate = dateTimeGenerator(), NewsId = r.NewsId, UserId = userId, Location = "null" };
                    context.Clicks.Add(click);
                }

                context.SaveChanges();

            }
        }
    }
}
