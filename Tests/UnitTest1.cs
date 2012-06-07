using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Worker;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class FillDatabaseTests
    {
        [TestMethod]
        public void TestRandomRows()
        {
            FillDatabase tests = new FillDatabase();
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();

            var count = 10;
            var rows = tests.getNumberOfRandomRows(context.NewsFiles, count).ToList();
            Assert.AreEqual(Math.Min(rows.Count, context.NewsFiles.Count()),count);
        }

        [TestMethod]
        public void TestRandomCategory()
        {
            FillDatabase tests = new FillDatabase();
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();

            var category = 10;
            var rows = tests.getRowsByCategory(context.NewsFiles, category, context.Categories).ToList();
            Assert.AreEqual(rows.All(x => x.CategoryId == category), true);
        }
    }
}
