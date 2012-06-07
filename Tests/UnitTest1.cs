using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Worker;
using System.Linq;
using MobilniPortalNovicLib.Models;
using System.Collections.Generic;
using MobilniPortalNovicLib.Personalize;
using MobilniPortalNovicLib.Helpers;

namespace Tests
{
    [TestClass]
    public class FillDatabaseTests
    {
        Category cat1 =
            new Category { CategoryId = 1, Name = "Šport", ParentCategoryId = new Nullable<int>() };
        Category cat2 =
            new Category { CategoryId = 2, Name = "Novice", ParentCategoryId = new Nullable<int>() };
        Category cat3 =
            new Category { CategoryId = 3, Name = "Zimski šport", ParentCategoryId = 1 };
        Category cat4 = new Category { CategoryId = 4, ParentCategoryId = new Nullable<int>() };
        Category cat5 = new Category { CategoryId = 5, ParentCategoryId = 3 };
        Category cat6 = new Category { CategoryId = 6, ParentCategoryId = 3 };
        List<Category> categoriesSmall;
        List<Category> categoriesBig;

        List<NewsFile> news;

        [TestInitialize]
        public void SetUp()
        {
            categoriesSmall = new List<Category> { cat1, cat2, cat3 };
            categoriesBig = new List<Category> { cat1, cat2, cat3, cat4, cat5, cat6 };
            news = new List<NewsFile>{
            new NewsFile{ NewsId=1, CategoryId=3,},
            new NewsFile{NewsId=2, CategoryId=2},
            new NewsFile{NewsId=3, CategoryId=1},
            new NewsFile{NewsId=6, CategoryId=2},
            new NewsFile{NewsId=7, CategoryId=1},
            new NewsFile{CategoryId=3, NewsId=4}};
        }

        [TestMethod]
        public void TestRandomRows1()
        {
            var count = 4;
            var rows = CategoryHelpers.getNumberOfRandomRows(news.AsQueryable(), count).ToList();
            Assert.AreEqual(rows.Count(), 4);
        }

        [TestMethod]
        public void TestCreateCategoryLookup()
        {
            var dict = CategoryHelpers.createCategoryParentLookup(categoriesSmall);

            var expected = new Dictionary<int, int>{
                {1,1},
                {2,2},
                {3,1}};

            CollectionAssert.AreEqual(dict, expected);

            //bigData
            dict = CategoryHelpers.createCategoryParentLookup(categoriesBig);

            expected = new Dictionary<int, int>{
                {1,1},
                {2,2},
                {3,1},
                {4,4},
                {5,1},
                {6,1}};

            CollectionAssert.AreEqual(dict, expected);
        }

        [TestMethod]
        public void TestCategoryChildrenLookup()
        {

            //smallTest
            var dict = CategoryHelpers.categoryChildrenLookup(categoriesSmall);


            var expected = new Dictionary<int, HashSet<Category>>{
                {1, new HashSet<Category>{cat1,cat3}},
                {2 ,new HashSet<Category>{cat2}},
                {3 ,new HashSet<Category>{cat3}}};


            foreach (var kv in dict)
            {
                Assert.IsTrue(expected.ContainsKey(kv.Key));
                CollectionAssert.AreEquivalent(kv.Value.ToList(), expected[kv.Key].ToList());
            }

            //Big test
            dict = CategoryHelpers.categoryChildrenLookup(categoriesBig);
            expected = new Dictionary<int, HashSet<Category>>{
                {1, new HashSet<Category>{cat1,cat3,cat5,cat6}},
                {2 ,new HashSet<Category>{cat2}},
                {3 ,new HashSet<Category>{cat3,cat5,cat6}},
                {4, new HashSet<Category>{cat4}},
                {5, new HashSet<Category>{cat5}},
                {6, new HashSet<Category>{cat6}}};


            foreach (var kv in dict)
            {
                Assert.IsTrue(expected.ContainsKey(kv.Key));
                CollectionAssert.AreEquivalent(kv.Value.ToList(), expected[kv.Key].ToList());
            }


        }



        [TestMethod]
        public void TestRandomCategory()
        {
            var category = 3;

            var rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categoriesSmall.AsQueryable()).ToList();


            Assert.AreEqual(rows.Count(), news.Where(x => x.CategoryId == 3).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 3), true);

            category = 1;
            rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categoriesSmall.AsQueryable()).ToList();
            Assert.AreEqual(rows.Count(), news.Where(x => x.CategoryId == 3 || x.CategoryId == 1).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 1 || x.CategoryId == 3), true);

            category = 3;
            rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categoriesBig.AsQueryable()).ToList();


            Assert.AreEqual(rows.Count(), news.Where(x => x.CategoryId == 3).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 3), true);

            category = 1;
            rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categoriesBig.AsQueryable()).ToList();
            Assert.AreEqual(rows.Count(), news.Where(x => x.CategoryId == 3 || x.CategoryId == 1).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 1 || x.CategoryId == 3), true);
        }
    }
}
