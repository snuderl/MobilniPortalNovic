using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Worker.Parsers
{
    static class WebHelper
    {
        public static HtmlDocument GetHtmlDocument(String uri, int timeout)
        {
            var req = (HttpWebRequest) WebRequest.Create(uri);
            req.Timeout = timeout;
            var response = req.GetResponse().GetResponseStream();
            response.ReadTimeout = timeout;

            HtmlDocument doc = new HtmlDocument();
            doc.Load(response, System.Text.Encoding.UTF8);

            return doc;
        }
    }
}
