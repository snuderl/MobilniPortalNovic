using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MobilniPortalNovicLib.Models;

namespace Worker.Parsers
{
    public class GenericNewsParser : INewsParser
    {
        private string containerId;
        public GenericNewsParser(string containerId)
        {
            this.containerId = containerId;
        }



        public IEnumerable<NewsFileExt> parseItem(IEnumerable<NewsFileExt> newsItems)
        {
            var news = newsItems.AsParallel().Select(x =>
            {
                return GetFullNewsFileInfo(x);
            });

            return news;
        }

        public IEnumerable<String> GetCategories(HtmlDocument doc)
        {
            var list = new List<String>();
            foreach (var i in doc.GetElementbyId("mcrumbfl").SelectNodes("a"))
            {
                list.Add(i.InnerHtml);
            }
            try
            {
                var last = doc.GetElementbyId("mcrumb").SelectNodes("//span[@class=\"item sel\"]").FirstOrDefault();
                if (last != null) { list.Add(last.InnerText); }
            }
            catch (Exception e)
            {
            }

            return list;
        }

        public NewsFileExt GetFullNewsFileInfo(NewsFileExt x)
        {
            var body = String.Empty;

            using (var c = new WebClient())
            {
                HtmlWeb web = new HtmlWeb();
                web.UseCookies = true;
                web.AutoDetectEncoding = true;
                HtmlDocument doc = web.Load(x.Link);
                try
                {
                    body = GetBody(doc);
                    x.Categories = GetCategories(doc);
                }
                catch (NullReferenceException e)
                {
                    body = "Error while fetching";
                    //Console.WriteLine("Null reference error at url:\n " + url);
                }

            }
            x.Content = body;
            return x;
        }


        public String GetBody(HtmlDocument doc)
        {
            var d = doc.GetElementbyId(containerId);
            foreach (var e in d.SelectNodes("//script"))
            {
                e.Remove();
            }
            foreach (var e in d.SelectNodes("//div[@class=\"rate\"]"))
            {
                e.Remove();
            }

            return d.InnerHtml;
        }
    }
}
