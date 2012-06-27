using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Personalize;

namespace MobilniPortalNovic.Helpers
{
    public class SessionCategories{
        public DateTime Created { get; set; }
        public int Duration { get; set; }
        
        public List<Category> Categories {get;set;}
    }

}