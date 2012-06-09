using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Worker.Parsers
{
    public class RssFeedParser : IFeedParser
    {
        public IEnumerable<NewsFileExt> parseFeed(MobilniPortalNovicLib.Models.Feed feed)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var lastUpdated = feed.LastUpdated;
                var doc = XDocument.Parse(client.DownloadString(feed.url));
                return parseRssDocument(doc, feed.FeedId);
            }
        }

        public IEnumerable<NewsFileExt> parseRssDocument(XDocument doc, int feedId)
        {
            IEnumerable<NewsFileExt> news = doc.Element("rss").Element("channel").Elements("item").Select(x => new NewsFileExt
                    {
                        Title = x.Element("title").Value,
                        ShortContent = ParsingHelpers.ExtractText(x.Element("description").Value),
                        PubDate = DateTime.Parse(x.Element("pubDate").Value),
                        Link = x.Element("link").Value,
                        FeedId = feedId
                    });
            return news;
        }
    }
}