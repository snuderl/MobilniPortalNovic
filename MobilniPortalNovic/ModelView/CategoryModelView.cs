using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class CategoryModelView
    {
        public Category Category { get; set; }
        public int NewsCount { get; set; }
    }
}