using System;

namespace Worker.Parsers
{
    internal class FeedItem
    {
        public String Title { get; set; }

        public String ShortContent { get; set; }

        public DateTime PubDate { get; set; }

        public String Url { get; set; }

        public int FeedId { get; set; }
    }
}