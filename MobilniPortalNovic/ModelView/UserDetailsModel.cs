using System;
using System.Collections.Generic;
using MobilniPortalNovicLib.Models;
using System.Linq;

namespace MobilniPortalNovic.ModelView
{
    public class UserDetailsModel
    {
        public String Username;
        public int id;
        public IEnumerable<ClickCounter> clicks;
        public Dictionary<String, int> feedStats;
        public Dictionary<String, int> categoryStats;
        public int MyProperty { get { return clicks.Count(); } }
    }
}