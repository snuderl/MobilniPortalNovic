﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Helpers
{
    internal class CategoryWraper
    {
        public ReadOnlyCollection<Category> Categories { get; private set; }

        public ReadOnlyCollection<int> LeafNodes { get; private set; }

        public Dictionary<int, int> ParentLookup { get; private set; }

        public Dictionary<int, HashSet<Category>> ChildrenLookup { get; private set; }

        public CategoryWraper(IEnumerable<Category> categories)
        {
            this.Categories = new ReadOnlyCollection<Category>(categories.ToList());
            ParentLookup = CategoryHelpers.createCategoryParentLookup(Categories);
            ChildrenLookup = CategoryHelpers.CategoryGetChildrensFromParent(categories);
            LeafNodes = new ReadOnlyCollection<int>(ChildrenLookup.Where(x => x.Value.Count == 1).Select(x => x.Key).ToList());
        }
    }
}