using System;
using System.Collections.Generic;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class UserDetailsModel
    {
        public String Username;
        public int id;
        public IEnumerable<ClickCounter> clicks;
        public Dictionary<String, float> feedStats;
        public Dictionary<String, float> categoryStats;
    }
}