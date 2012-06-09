using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MobilniPortalNovicContext12>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MobilniPortalNovicContext12 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.NewsSites.AddOrUpdate(
                new NewsSite { Name = "Siol.net", SiteId = 1 }
                );

            context.Feeds.AddOrUpdate(
                new Feed { FeedName = "Sportal", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=sportal" },
                new Feed { FeedName = "Scena", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=scena" },
                new Feed { FeedName = "Avtomoto", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=avtomoto" },
                new Feed { FeedName = "Trendi", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=trendi" },
                new Feed { FeedName = "Kultura", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=kultura" },
            new Feed { FeedName = "TV & Video", LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=tv" }
           , new Feed
           {
               FeedName = "Novice",
               LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"),
               NewsSiteId = 1,
               url = @"http://www.siol.net/rss.aspx?path=novice"
           }
                );

            context.Users.AddOrUpdate(
                new User { Username = "snuderl", UserId = 1 },
                new User { Username = "Blaž", UserId = 2 },
                new User { Username = "Anja", UserId = 3 },
                new User { Username = "Matej", UserId = 4 },
                new User { Username = "Samo", UserId = 5 }
                );
        }
    }
}