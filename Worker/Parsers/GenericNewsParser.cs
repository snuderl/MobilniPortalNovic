using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MobilniPortalNovicLib.Models;
using System.Collections.Concurrent;

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
            ConcurrentBag<NewsFileExt> bag = new ConcurrentBag<NewsFileExt>();
            Parallel.ForEach(newsItems, x =>
            {
                try
                {
                    bag.Add(GetFullNewsFileInfo(x));
                }
                catch (Exception e)
                {

                }
            });
            return bag.ToList();
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
                body = GetBody(doc);
                x.Categories = GetCategories(doc);

            }
            x.Content = body;
            return x;
        }




        public String GetBody(HtmlDocument doc)
        {
            var d = doc.GetElementbyId(containerId);

            //Remove nodes
            //d = RemoveNodes(d, SelectTags("script"));
            //d = RemoveNodes(d, SelectTags("div", "class", "rate"));
            //Header
            d = RemoveNodes(d, SelectTags("header"));
            //Similiar articles
            d = RemoveNodes(d, SelectTags("article", "class", "wnd_news_std"));

            StringBuilder sb = new StringBuilder();

            RecursiveSearch(d, sb);


            return sb.ToString();

        }

        public void RecursiveSearch(HtmlNode node, StringBuilder sb)
        {
            foreach (var child in node.ChildNodes)
            {
                if (Regex.IsMatch(child.Name, "^h.$") || child.Name == "p")
                {
                    sb.Append(ParsingHelpers.ExtractText(child.InnerHtml));
                }
                else if (child.HasChildNodes)
                {
                    RecursiveSearch(child, sb);
                }
            }
        }

        public HtmlNode RemoveNodes(HtmlNode node, String xpath)
        {
            foreach (var e in node.SelectNodes(xpath))
            {
                e.Remove();
            }
            return node;
        }

        public String SelectTags(string tagname, string attributeName = null, string attributeValue = null)
        {
            StringBuilder sb = new StringBuilder("//" + tagname);
            if (attributeName != null)
            {
                sb.Append("[@");
                sb.Append(attributeName);
                if (attributeValue != null)
                {
                    sb.Append("=\"");
                    sb.Append(attributeValue);
                }
                sb.Append("\"]");
            }
            return sb.ToString();
        }
    }
}
