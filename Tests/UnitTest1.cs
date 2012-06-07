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
        List<Category> categories = new List<Category> {
            new Category { CategoryId = 1, Name = "Šport"  ,ParentCategoryId=new Nullable<int>()},
            new Category { CategoryId = 2, Name = "Novice"  ,ParentCategoryId=new Nullable<int>()},
            new Category { CategoryId = 3, Name = "Zimski šport", ParentCategoryId = 1} };

        List<NewsFile> news = new List<NewsFile>{
            new NewsFile{ NewsId=1, CategoryId=3,},
            new NewsFile{NewsId=2, CategoryId=2},
            new NewsFile{NewsId=3, CategoryId=1},
            new NewsFile{NewsId=6, CategoryId=2},
            new NewsFile{NewsId=7, CategoryId=1},
            new NewsFile{CategoryId=3, NewsId=4}};


        [TestMethod]
        public void TestRandomRows1()
        {
            var count = 4;
            var rows = CategoryHelpers.getNumberOfRandomRows(news.AsQueryable(), count).ToList();
            Assert.AreEqual(rows.Count(),4);
        }

        [TestMethod]
        public void TestCreateCategoryLookup()
        {
            CategoryPersonalizer p = new CategoryPersonalizer(new MobilniPortalNovicContext12());
            var dict = CategoryHelpers.createCategoryParentLookup(categories);

            var expected = new Dictionary<int, int>{
                {1,1},
                {2,2},
                {3,1}};

            CollectionAssert.AreEqual(dict, expected);
        }

        [TestMethod]
        public void TestCategoryChildrenLookup()
        {
            var dict = CategoryHelpers.categoryChildrenLookup(categories);

            var expected = new Dictionary<int, HashSet<int>>{
                {1, new HashSet<int>{1,3}},
                {2 ,new HashSet<int>{2}},
                {3 ,new HashSet<int>{3}}};


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

            var rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categories.AsQueryable()).ToList();


            Assert.AreEqual(rows.Count(), news.Where(x=>x.CategoryId==3).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 3), true);

            category = 1;
            rows = CategoryHelpers.getRowsByCategory(news.AsQueryable(), category, categories.AsQueryable()).ToList();
            Assert.AreEqual(rows.Count(), news.Where(x=>x.CategoryId==3||x.CategoryId==1).Count());
            Assert.AreEqual(rows.All(x => x.CategoryId == 1 || x.CategoryId==3), true);
        }
    }
}
