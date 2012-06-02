//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using HtmlAgilityPack;
//using MobilniPortalNovicLib.Models;

//namespace MobilniPortalNovicLib
//{
//    public class RssParser
//    {
//        public IEnumerable<XDocument> parseRss(Feed feed)
//        {
//            using (var client = new WebClient())
//            {
//                var lastUpdated = feed.LastUpdated;
//                var doc = XDocument.Parse(client.DownloadString(feed.url));
//                IEnumerable<NewsFile> news = doc.Element("rss").Element("channel").Elements("item").AsParallel().
//                Select(x => new NewsFile
//                                {
//                                    Title = x.Element("title").Value,
//                                    ShortContent = x.Element("description").Value,
//                                    PubDate = DateTime.Parse(x.Element("pubDate").Value),
//                                    Content = x.Element("link").Value,
//                                    FeedId = feed.FeedId

//                                }).Select(x =>
//                                {
//                                    x.Content = fullDescription(x.Content);
//                                    return x;
//                                });

//                return news;
//            }
//        }

//        public String fullDescription(String url)
//        {
//            var body = String.Empty;
//            using (var c = new WebClient())
//            {
//                HtmlWeb web = new HtmlWeb();
//                web.UseCookies = true;
//                HtmlDocument doc = web.Load(url);
//                try
//                {
//                    body = doc.GetElementbyId("article").InnerHtml;
//                }
//                catch (NullReferenceException e)
//                {
//                    Console.WriteLine("Null reference error at url:\n " + url);
//                }

//            }
//            return body;
//        }
//    }
//}
