using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class UserDetailsModel
    {
        public String Username;
        public int id;
        public IEnumerable<ClickCounter> clicks;
        public Dictionary<String, float> categoryStats;

    }
}