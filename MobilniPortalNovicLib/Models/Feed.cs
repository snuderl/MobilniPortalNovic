using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public String url { get; set; }

        public String FeedName { get; set; }

        [ForeignKey("NewsSite")]
        public int NewsSiteId { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }


        public virtual NewsSite NewsSite { get; set; }
    }

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public String Name { get; set; }

        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        [InverseProperty("ParentCategory")]
        public virtual ICollection<Category> Children { get; set; }
    }

    public class NewsFile
    {
        [Key]
        public int NewsId { get; set; }

        [Required]
        public String ShortContent { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public String Content { get; set; }

        [Required]
        public DateTime PubDate { get; set; }

        [Required]
        public int FeedId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public String Link { get; set; }

        public virtual Category Category { get; set; }

        public virtual Feed Feed { get; set; }
    }

    public class ClickCounter
    {
        [Key]
        public int ClickId { get; set; }

        [Required]
        public DateTime ClickDate { get; set; }

        
        public String Location { get; set; }

        [Required]
        public int NewsId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual NewsFile NewsFile { get; set; }

        public virtual User User { get; set; }

        public virtual Category Category { get; set; }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public String Username { get; set; }

        public virtual IEnumerable<ClickCounter> Clicks { get; set; }
    }
}