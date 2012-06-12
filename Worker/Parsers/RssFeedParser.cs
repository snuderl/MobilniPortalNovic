using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BondiGeek.Logging;
using HtmlAgilityPack;

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
                return parseRssDocument(doc, feed.FeedId);
            }
            catch (Exception e)
            {
                    String error = "Failed parsing feed: " + feed.FeedName;
                    LogWriter.Instance.Log(error);
                    return new List<NewsFileExt>();
            }
        }

        public IEnumerable<NewsFileExt> parseRssDocument(HtmlDocument doc, int feedId)
        {
            var root = doc.DocumentNode;
            var items = root.SelectNodes("//item");
            IEnumerable<NewsFileExt> news = items.Select(x => NodeToNewsFile(x, feedId));
            return news;
        }

        public NewsFileExt NodeToNewsFile(HtmlNode x, int feedId)
        {
            var n = new NewsFileExt();
            n.Title = x.Element("title").InnerHtml;
            n.ShortContent = ParsingHelpers.ExtractText(x.Element("description").InnerHtml);
            n.PubDate = DateTime.Parse(x.Element("pubdate").InnerHtml);
            n.Link = x.Element("link").InnerHtml;
            n.FeedId = feedId;
            return n;
        }
    }
}