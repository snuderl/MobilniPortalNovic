using System;
using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class UserDetailsModel
    {
        public String Username { get; set; }

        public int Id { get; set; }

        public IEnumerable<ClickCounter> clicks;
        public Dictionary<String, int> feedStats;
        public Dictionary<Category, int> categoryStats;

        public int MyProperty { get { return clicks.Count(); } }
    }
}