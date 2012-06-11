using System;
using System.Collections.Generic;
using MobilniPortalNovicLib.Models;

namespace Worker.Parsers
{
    public class NewsFileExt : NewsFile
    {
        public IEnumerable<String> Categories { get; set; }
        public Boolean ParseOk { get; set; }
    }
}