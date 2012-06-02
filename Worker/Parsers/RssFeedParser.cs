using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MobilniPortalNovicLib.Models;

namespace Worker.Parsers
{
    class RssFeedParser : IFeedParser
    {

        public IEnumerable<NewsFile> parseFeed(MobilniPortalNovicLib.Models.Feed feed)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var lastUpdated = feed.LastUpdated;
                var doc = XDocument.Parse(client.DownloadString(feed.url));
                IEnumerable<NewsFile> news = doc.Element("rss").Element("channel").Elements("item").Select(x => new NewsFile
                                {
                                    Title = x.Element("title").Value,
                                    ShortContent = x.Element("description").Value,
                                    PubDate = DateTime.Parse(x.Element("pubDate").Value),
                                    Content=x.Element("link").Value,
                                    FeedId = feed.FeedId

                                });
                return news;
            }
        }
    }
}
