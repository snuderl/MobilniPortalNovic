using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BondiGeek.Logging;

namespace Worker.Parsers
{
    public class RssFeedParser : IFeedParser
    {
        public IEnumerable<NewsFileExt> parseFeed(MobilniPortalNovicLib.Models.Feed feed)
        {
            try
            {
                var doc = WebHelper.GetHtmlDocument(feed.url, 3000);

                var lastUpdated = feed.LastUpdated;

                return parseRssDocument(XDocument.Parse(doc), feed.FeedId);
            }
            catch (Exception e)
            {
                String error = "Failed parsing feed: " + feed.FeedName;
                LogWriter.Instance.Log(error);
                return new List<NewsFileExt>();
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