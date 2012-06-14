﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BondiGeek.Logging;
using HtmlAgilityPack;

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
                    var a = GetFullNewsFileInfo(x);
                    a.ParseOk = true;
                    bag.Add(a);
                }
                catch (System.NullReferenceException)
                {
                    x.ParseOk = false;
                    
                    bag.Add(x);
                }
                catch (Exception)
                {
                    String error = "Error getting link: " + x.Link;
                    LogWriter.Instance.Log(error);
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

            //var web = new HtmlWeb();
            //web.AutoDetectEncoding = true;
            //var doc = web.Load(x.Link);
            var html = WebHelper.GetHtmlDocument(x.Link, 15000);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);


            body = GetBody(doc);
            x.Categories = GetCategories(doc);

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

            //Remove preberite tudi
            String[] textToRemove = { "Preberite tudi:", "Preberite še:", "Prebertie še:" };
            d.Elements("h3").Where(x => textToRemove.Contains(x.InnerHtml)).ToList().ForEach(x =>
            {
                x.Remove();
            });

            StringBuilder sb = new StringBuilder();

            RecursiveSearch(d, sb);

            return sb.ToString();
        }

        public void RecursiveSearch(HtmlNode node, StringBuilder sb)
        {
            foreach (var child in node.ChildNodes)
            {
                if (Regex.IsMatch(child.Name, "^h.$"))
                {
                    sb.Append("<" + child.Name + ">");
                    sb.Append(ParsingHelpers.ExtractText(child.InnerHtml));
                    sb.Append("</" + child.Name + ">");
                }
                else if (child.Name == "p")
                {
                    sb.Append("<p>");
                    String c = ParsingHelpers.ExtractText(child.InnerHtml);
                    sb.Append(WrapIfHasOnlyGivenChildNode(child, c, "b"));
                    sb.Append("</p>");
                }
                else if (child.Name == "img")
                {
                    var s = child.OuterHtml;
                    sb.Append(s);
                }
                else if (child.HasChildNodes)
                {
                    RecursiveSearch(child, sb);
                }
            }
        }

        public String WrapIfHasOnlyGivenChildNode(HtmlNode node, String textToWrap, String NodeName)
        {
            if (node.ChildNodes.Count() == 1 && node.ChildNodes[0].Name == NodeName)
            {
                return "<" + NodeName + ">" + textToWrap + "</" + NodeName + ">";
            }
            return textToWrap;
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