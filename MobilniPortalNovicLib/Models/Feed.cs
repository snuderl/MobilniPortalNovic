using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MobilniPortalNovicLib.Helpers;

namespace MobilniPortalNovicLib.Models
{
    public class Coordinates
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }


    }

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
        
        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public Coordinates Coordinates { get {
            return Longitude == null || Latitude == null ?
                null :
                new Coordinates { Longitude = this.Longitude.Value, Latitude = this.Latitude.Value
                };
        } }

        public String DisplayCoordinates()
        {
            if (Longitude == null || Latitude == null)
            {
                return "null";
            }
            return this.Longitude + "|" + this.Latitude;
        }

        public void SetLocation(Coordinates coord)
        {
            if (coord == null)
            {
                Longitude = null;
                Latitude = null;
            }
            else
            {
                Longitude = coord.Longitude;
                Latitude = coord.Latitude;
            }
        }

        [Required]
        public int NewsId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Range(1, 7)]
        public int DayOfWeek { get; set; }

        [Range(1, 1440)]
        public int TimeOfDay { get; set; }

        public int CategoryId { get; set; }

        public virtual NewsFile NewsFile { get; set; }

        public virtual User User { get; set; }

        public virtual Category Category { get; set; }

        public void SetDayOfWeekAndTimeOfDay()
        {
            DayOfWeek = DateTimeHelpers.weekDays.IndexOf(ClickDate.DayOfWeek) + 1;
            TimeOfDay = (int)ClickDate.TimeOfDay.TotalMinutes;
        }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public String Username { get; set; }

        [Required, DataType(DataType.Password)]
        public String Password { get; set; }

        public Guid AccessToken { get; set; }

        public virtual IEnumerable<ClickCounter> Clicks { get; set; }
    }
}