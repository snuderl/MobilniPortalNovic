using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobilniPortalNovicLib.Models;

namespace Worker.Parsers
{
    public interface IFeedParser
    {
        IEnumerable<NewsFile> parseFeed(Feed feed);
    }

    public interface INewsParser
    {
        IEnumerable<NewsFile> parseItem(IEnumerable<NewsFile> items);
    }
}
