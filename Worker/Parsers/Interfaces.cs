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
        IEnumerable<NewsFileExt> parseFeed(Feed feed);
    }

    public interface INewsParser
    {
        IEnumerable<NewsFileExt> parseItem(IEnumerable<NewsFileExt> items);
    }
}
