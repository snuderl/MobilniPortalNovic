using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Worker;

namespace WorkerTests
{
    public class FillDatabaseTests
    {
        [Fact]
        public void TestRandomRows()
        {
            FillDatabase tests = new FillDatabase();
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();

            var count = 10;
            var rows = tests.getNumberOfRandomRows(context.NewsFiles, count).ToList();
            Assert.True(Math.Min(rows.Count, context.NewsFiles.Count()) == count);
        }

        [Fact]
        public void TestRandomCategory()
        {
            FillDatabase tests = new FillDatabase();
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();

            var category = 10;
            var rows = tests.getRowsByCategory(context.NewsFiles, category, context.Categories).ToList();
            Assert.True(rows.All(x => x.CategoryId == category));
        }
    }
}
