using System.Collections.Generic;
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