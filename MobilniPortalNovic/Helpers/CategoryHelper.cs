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
            if (category.ParentCategoryId != null && (category.Name == "Novice" || category.Name.Equals("Novice")))
            {
                return category.ParentCategory.Name + " novice";
            }
            return category.Name;
        }
    }
}