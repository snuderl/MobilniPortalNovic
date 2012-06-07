using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Helpers
{
    public class CategoryHelpers
    {

        static public Dictionary<int, int> createCategoryParentLookup(List<Category> categories)
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

        static public Dictionary<int, HashSet<int>> categoryChildrenLookup(List<Category> categories)
        {
            var dict = createCategoryParentLookup(categories);
            var lookup = new Dictionary<int, HashSet<int>>();
            foreach (var s in dict)
            {
                if (!lookup.ContainsKey(s.Key))
                {
                    lookup[s.Key] = new HashSet<int> { s.Key };
                }
                else
                {
                    lookup[s.Key].Add(s.Key);
                }
                if (!lookup.ContainsKey(s.Value))
                {
                    lookup[s.Value] = new HashSet<int>();
                }
                lookup[s.Value].Add(s.Key);
            }

            return lookup;
        }

        static public IQueryable<NewsFile> getRowsByCategory(IQueryable<NewsFile> query, int category, IQueryable<Category> categories)
        {
            var categoryChildrenDict = CategoryHelpers.categoryChildrenLookup(categories.ToList());
            var goodCategories = categoryChildrenDict[category];
            return query.Where(x => goodCategories.Contains(x.CategoryId));
        }

        static public IQueryable<T> getNumberOfRandomRows<T>(IQueryable<T> query, int count)
        {


            return query.OrderBy(x => Guid.NewGuid()).Take(count);
        }
    }
}
