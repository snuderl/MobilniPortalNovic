using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Parsers
{
    class GenericNewsParser : INewsParser
    {
        private string containerId;
        public GenericNewsParser(string containerId)
        {
            this.containerId = containerId;
        }



        public IEnumerable<Models.NewsFile> parseItem(IEnumerable<NewsFile> newsItems)
        {
            var news = newsItems.AsParallel().Select(x =>
            {
                x.Content = fullDescription(x.Content);
                return x;
            });

            return news;
        }

        public String fullDescription(String url)
        {
            var body = String.Empty;
            using (var c = new WebClient())
            {
                HtmlWeb web = new HtmlWeb();
                web.UseCookies = true;
                web.AutoDetectEncoding = true;
                HtmlDocument doc = web.Load(url);
                try
                {
                    body = doc.GetElementbyId(containerId).InnerHtml;
                }
                catch (NullReferenceException e)
                {
                    body = "Error while fetching";
                    Console.WriteLine("Null reference error at url:\n " + url);
                }

            }
            return body;
        }
    }
}
