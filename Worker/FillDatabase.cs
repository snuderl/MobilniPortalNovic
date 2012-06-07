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
                var rows = CategoryHelpers.getRowsByCategory(context.NewsFiles.Include("Category"), Category, context.Categories).OrderBy(x => new Guid()).Take(count).ToList();
                foreach (var r in rows)
                {
                    var click = new ClickCounter { CategoryId = r.CategoryId, ClickDate = dateTimeGenerator(), NewsId = r.NewsId, UserId = userId, Location = "null" };
                    context.Clicks.Add(click);
                }

                context.SaveChanges();

            }
        }
    }
}
