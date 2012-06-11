using System;
using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Helpers
{
    public class CategoryHelpers
    {

        /// <summary>
        /// Returns all parents for each category
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        static public Dictionary<int, int> createCategoryParentLookup(IEnumerable<Category> categories)
        {
            var dict = new Dictionary<int, int>();
            foreach (var i in categories)
            {
                if (i.ParentCategoryId == null)
                {
                    dict.Add(i.CategoryId, i.CategoryId);
                }
                else
                {
                    var c = i;
                    while (c.ParentCategoryId != null)
                    {
                        c = categories.Where(x => x.CategoryId == c.ParentCategoryId).First();
                    }
                    dict.Add(i.CategoryId, c.CategoryId);
                }
            }
            return dict;
        }


        /// <summary>
        /// Builds up category -> parent lookup table
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        static public Dictionary<int, int> categoryParentImidiateLookup(IEnumerable<Category> categories)
        {
            var dict = new Dictionary<int, int>();
            foreach (var i in categories)
            {
                if (i.ParentCategoryId != null)
                {
                    dict[i.CategoryId] = i.ParentCategoryId.Value;
                }
            }
            return dict;
        }


        /// <summary>
        /// Get all children for each category
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        static public Dictionary<int, HashSet<Category>> CategoryGetChildrensFromParent(IEnumerable<Category> categories)
        {
            var lookup = categories.ToDictionary(x => x.CategoryId, x => new HashSet<Category> { x });
            for (int i = 0; i < categories.Count(); i++)
            {
                foreach (var l in categories)
                {
                    if (l.ParentCategoryId != null)
                    {
                        foreach (var z in lookup[l.CategoryId])
                        {
                            lookup[l.ParentCategoryId.Value].Add(z);
                        }
                    }
                }
            }
            return lookup;
        }


        /// <summary>
        /// Get all parents for each category
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        static public Dictionary<int, HashSet<Category>> CategoryGetParentsFromChildren(IEnumerable<Category> categories)
        {
            var lookup = categories.ToDictionary(x => x.CategoryId, x => new HashSet<Category> { x });
            for (int i = 0; i < categories.Count(); i++)
            {
                foreach (var l in categories)
                {
                    if (l.ParentCategoryId != null)
                    {
                        foreach (var z in lookup[l.ParentCategoryId.Value])
                        {
                            lookup[l.CategoryId].Add(z);
                        }
                    }
                }
            }
            return lookup;
        }


        /// <summary>
        /// Returns all rows for given category including its children
        /// </summary>
        /// <param name="query"></param>
        /// <param name="category"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        static public IQueryable<NewsFile> getRowsByCategory(IEnumerable<NewsFile> query, int category, IEnumerable<Category> categories)
        {
            var categoryChildrenDict = CategoryHelpers.CategoryGetChildrensFromParent(categories.ToList());
            var goodCategories = categoryChildrenDict[category].Select(x => x.CategoryId);
            return query.AsQueryable().Where(x => goodCategories.Contains(x.CategoryId));
        }

        /// <summary>
        /// Returns number of items per category, including children
        /// </summary>
        /// <returns></returns>
        static public Dictionary<int, int> GetNumberOfItemsPerCategory(IEnumerable<NewsFile> news, IEnumerable<Category> categories)
        {
            var cat = categories.ToList();
            var dict = CategoryGetChildrensFromParent(cat);


            var newsLookup = news.GroupBy(x => x.CategoryId).ToDictionary(x => x.Key, x => x.Count());





            var CountDictionary = new Dictionary<int, int>(cat.ToDictionary(x => x.CategoryId, x => 0));

            Queue<int> queue = new Queue<int>();
            dict.Where(x => x.Value.Count() == 1).ToList().ForEach(x => queue.Enqueue(x.Key));

            while (queue.Count > 0)
            {
                int i = queue.Dequeue();
                var children = dict[i].ToList();

                int count = 0;
                children.ForEach(x =>
                {
                    if (newsLookup.ContainsKey(x.CategoryId))
                    {
                        count += newsLookup[x.CategoryId];
                    }
                });
                CountDictionary[i] = count;

                var c = cat.Where(x => x.CategoryId == i).First();
                if (c.ParentCategoryId != null)
                {
                    queue.Enqueue(c.ParentCategoryId.Value);
                }
            }

            return CountDictionary;
        }

    }
}