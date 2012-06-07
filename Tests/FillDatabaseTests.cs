using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;
using Worker;

namespace Tests
{
    [TestClass]
    class FillDatabaseTests
    {

        [TestMethod]
        public void SimulateClicksTest()
        {
            var context = new MobilniPortalNovicContext12();
            FillDatabase f = new FillDatabase(context);
            var d = DateTime.Now;
            var categoryId = 2;
            var userId = 1;
            var count = 10;
            var query = f.SimulateClicks(userId, categoryId, count, () => d);
            var dict = CategoryHelpers.CategoryGetChildrensFromParent(context.Categories.ToList());

            foreach (var r in query)
            {
                context.Clicks.Remove(r);
            }
            context.SaveChanges();

            foreach (var r in query)
            {
                Assert.IsTrue(r.UserId == 1);
                Assert.IsTrue(dict[categoryId].Select(x => x.CategoryId).Contains(r.CategoryId));
                Assert.AreEqual(d, r.ClickDate);
            }

        }
    }
}
