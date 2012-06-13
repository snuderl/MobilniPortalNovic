using System;
using System.IO;
using System.Net;
using System.Text;

namespace Worker.Parsers
{
    internal static class WebHelper
    {
        public static String GetHtmlDocument(String uri, int timeout)
        {
            var req = (HttpWebRequest)WebRequest.Create(uri);
            req.Timeout = timeout;
            var response = req.GetResponse().GetResponseStream();
            response.ReadTimeout = timeout;

            var reader = new StreamReader(response, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }
}