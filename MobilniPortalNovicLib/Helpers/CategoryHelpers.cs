using System;
using System.Collections.Generic;
using System.Linq;
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

        static public Dictionary<int, int> categoryParentImidiateLookup(List<Category> categories)
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

        static public Dictionary<int, HashSet<Category>> CategoryGetChildrensFromParent(List<Category> categories)
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

        static public Dictionary<int, HashSet<Category>> CategoryGetParentsFromChildren(List<Category> categories)
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

        static public IQueryable<NewsFile> getRowsByCategory(IQueryable<NewsFile> query, int category, IQueryable<Category> categories)
        {
            var categoryChildrenDict = CategoryHelpers.CategoryGetChildrensFromParent(categories.ToList());
            var goodCategories = categoryChildrenDict[category].Select(x => x.CategoryId);
            return query.Where(x => goodCategories.Contains(x.CategoryId));
        }

        static public IQueryable<NewsFile> getNumberOfRandomRows(IQueryable<NewsFile> query, int count)
        {
            return query.OrderBy(x => Guid.NewGuid()).Take(count);
        }
    }
}