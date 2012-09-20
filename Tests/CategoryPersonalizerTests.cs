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
            var date = DateTime.Parse("2012-6-14T15:35:00.0000000Z");
            categoriesSmall = new List<Category> { cat1, cat2, cat3 };
            clicks = new List<ClickCounter>{
                new ClickCounter{ NewsId=1, NewsFile=n1, ClickDate = date, Longitude=1, Latitude=1},
                new ClickCounter{ NewsId=2, NewsFile=n2, ClickDate = date.AddHours(2), Longitude=1, Latitude=1},
                new ClickCounter{ NewsId=3, NewsFile=n3, ClickDate = date.AddHours(-3), Longitude=4, Latitude=4},
                new ClickCounter{ NewsId=4, NewsFile=n4, ClickDate = date.AddDays(1), Longitude=1, Latitude=21},
                new ClickCounter{ NewsId=5, NewsFile=n5, ClickDate = date.AddDays(2), Longitude=1, Latitude=null},
                new ClickCounter{ NewsId=6, NewsFile=n6, ClickDate = date.AddDays(3).AddHours(2), Longitude=null, Latitude=1}
            };

        }

        public void FilterClicksByDateTest()
        {
            var dateTime = DateTime.Parse("2012-6-14T15:35:00.0000000Z");
            var TimeOfDayFilter = new TimeOfDayFilter(dateTime);
            clicks.ForEach(x => x.SetDayOfWeekAndTimeOfDay());
            var result = TimeOfDayFilter.Filter(clicks.AsQueryable());
            Assert.AreEqual(result.Count(), 3);

            result = TimeOfDayFilter.Filter(clicks.AsQueryable());
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod]
        public void NClosestLocations()
        {
            var dateTime = DateTime.Parse("2012-6-14T15:35:00.0000000Z");
            clicks.ForEach(x => x.SetDayOfWeekAndTimeOfDay());
            var result = CategoryPersonalizer.FilterByNNearestClicks(clicks.AsQueryable(), 2, new Coordinates { Longitude = 1, Latitude = 1 });
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.Contains(clicks[0]), true);
            Assert.AreEqual(result.Contains(clicks[1]), true);

            result = CategoryPersonalizer.FilterByNNearestClicks(clicks.AsQueryable(), 5, new Coordinates { Longitude = 1, Latitude = 1 });
            Assert.AreEqual(result.Count(), 4);
            Assert.AreEqual(result.Contains(clicks[0]), true);
            Assert.AreEqual(result.Contains(clicks[1]), true);


            result = CategoryPersonalizer.FilterByNNearestClicks(clicks.AsQueryable(), 1, new Coordinates { Longitude =20, Latitude = 20 });
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.Contains(clicks[3]), true);
        }
    }
}