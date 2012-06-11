using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilniPortalNovic.ModelView
{
    public class HomePageModel
    {
        public int NewsFileCount { get; set; }
        public int CategoriesCount { get; set; }
        public DateTime NewsLastUpdated { get; set; }
    }
}