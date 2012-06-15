using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class RssHtmlModelView
    {
        public IEnumerable<NewsFile> newsFiles;
        public IEnumerable<Category> includedCategories;
    }
}