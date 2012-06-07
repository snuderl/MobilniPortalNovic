﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.ModelView
{
    public class ClickModelView
    {
        public Dictionary<int, HashSet<Category>> CategoryParents { get; set; }
        public IEnumerable<ClickCounter> Clicks { get; set; }
    }
}