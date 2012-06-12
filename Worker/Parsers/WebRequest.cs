using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Worker.Parsers
{
    static class WebHelper
    {
        public static String GetHtmlDocument(String uri, int timeout)
        {
            var req = (HttpWebRequest) WebRequest.Create(uri);
            req.Timeout = timeout;
            var response = req.GetResponse().GetResponseStream();
            response.ReadTimeout = timeout;

            var reader = new StreamReader(response, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }
}
