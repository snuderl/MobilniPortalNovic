using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Helpers
{
    public static class CategoryHelper
    {
        public static String Display(this Category category)
        {
            List<String> replace = new List<String> { "Novice", "Drugo" };
            if (category.ParentCategoryId != null)
            {
                if (replace.Contains(category.Name))
                {
                    return category.ParentCategory.Name + " " + category.Name.ToLower();
                }
            }
            return category.Name;
        }
    }
}