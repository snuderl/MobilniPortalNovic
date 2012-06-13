using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Personalize;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class CategoryPersonalizerTests
    {
        private Category cat1 =
            new Category { CategoryId = 1, Name = "Šport", ParentCategoryId = new Nullable<int>() };

        private Category cat2 =
            new Category { CategoryId = 2, Name = "Novice", ParentCategoryId = new Nullable<int>() };

        private Category cat3 =
            new Category { CategoryId = 3, Name = "Novice", ParentCategoryId = 1 };

        private Category cat4 = new Category { CategoryId = 4, ParentCategoryId = new Nullable<int>() };
        private Category cat5 = new Category { CategoryId = 5, ParentCategoryId = 3 };
        private Category cat6 = new Category { CategoryId = 6, ParentCategoryId = 3 };
        NewsFile n1 = new NewsFile { NewsId = 1, CategoryId = 3 };
        NewsFile n2 = new NewsFile { NewsId = 2, CategoryId = 2 };
        NewsFile n3 = new NewsFile { NewsId = 3, CategoryId = 1 };
        NewsFile n4 = new NewsFile { NewsId = 4, CategoryId = 2 };
        NewsFile n5 = new NewsFile { NewsId = 5, CategoryId = 1 };
        NewsFile n6 = new NewsFile { NewsId = 6, CategoryId = 2 };
        private List<Category> categoriesSmall;
        private List<ClickCounter> clicks;

        [TestMethod]
        public void MustAlwaysHaveAtleastCategoryWithMaxClicks()
        {
            var good = CategoryPersonalizer.GetDesiredCategories(clicks, 0.00001f);
            Assert.AreEqual(good.Count, 1);
            Assert.IsTrue(good.Contains(2));
        }

        [TestMethod]
        public void ContainsMoreThanOneCategory()
        {
            var good = CategoryPersonalizer.GetDesiredCategories(clicks, 60f);
            Assert.AreEqual(good.Count, 2);
            Assert.IsTrue(good.Contains(2));
            Assert.IsTrue(good.Contains(1));
        }

        [TestInitialize]
        public void SetUp()
        {
            categoriesSmall = new List<Category> { cat1, cat2, cat3 };
            clicks = new List<ClickCounter>{
                new ClickCounter{ NewsId=1, NewsFile=n1, ClickDate = DateTime.Now},
                new ClickCounter{ NewsId=2, NewsFile=n2, ClickDate = DateTime.Now.AddHours(2)},
                new ClickCounter{ NewsId=3, NewsFile=n3, ClickDate = DateTime.Now.AddHours(-3)},
                new ClickCounter{ NewsId=4, NewsFile=n4, ClickDate = DateTime.Now.AddDays(1)},
                new ClickCounter{ NewsId=5, NewsFile=n5, ClickDate = DateTime.Now.AddDays(2)},
                new ClickCounter{ NewsId=6, NewsFile=n6, ClickDate = DateTime.Now.AddDays(3).AddHours(2)}
            };

        }

        [TestMethod]
        public void FilterClicksByDateTest()
        {
            clicks.ForEach(x => x.SetDayOfWeekAndTimeOfDay());
            var result = CategoryPersonalizer.FilterClicksByDate(clicks.AsQueryable(), DateTime.Now);
            Assert.AreEqual(result.Count(), 2);
        }
    }
}