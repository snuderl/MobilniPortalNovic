using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace Worker.Parsers
{
    public class NewsFileExt : NewsFile
    {
        public IEnumerable<String> Categories { get; set; }
    }
}
