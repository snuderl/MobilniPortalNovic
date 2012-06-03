using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobilniPortalNovicLib.Models
{
    public class NewsSite
    {
        [Key]
        public int SiteId { get; set; }
        public String Name { get; set; }

        public virtual ICollection<Feed> Feeds { get; set; }

    }
    public class Feed
    {
        [Key]
        public int FeedId { get; set; }
        public String url { get; set; }
        public int CategoryId { get; set; }
        
        [ForeignKey("NewsSite")]
        public int NewsSiteId { get; set; }
        public DateTime LastUpdated { get; set; }


        public virtual Category Category { get; set; }
        public virtual NewsSite NewsSite { get; set; }
    }

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public String Name { get; set; }
    }

    public class NewsFile
    {
        [Key]
        public int NewsId { get; set; }

        public String ShortContent { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public DateTime PubDate { get; set; }
        public int FeedId { get; set; }

        public virtual Feed Feed { get; set; }
    }

    public class ClickCounter
    {
        [Key]
        public int ClickId { get; set; }

        public DateTime ClickDate { get; set; }
        public String Location { get; set; }

        public int NewsId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        public virtual NewsFile NewsFile { get; set; }
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        public String Username { get; set; }

        public virtual IEnumerable<ClickCounter> Clicks { get; set; }
    }
}
