using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            context.Categories.AddOrUpdate(
               new Category { CategoryId = 1, Name = "Šport" },
               new Category { CategoryId = 2, Name = "Scena" },
               new Category { CategoryId = 3, Name = "Avtomoto" },
               new Category { CategoryId = 4, Name = "Trendi" },
               new Category { CategoryId = 5, Name = "Kultura" }
               );

            context.NewsSites.AddOrUpdate(
                new NewsSite { Name = "Siol.net", SiteId = 1 }
                );

            context.Feeds.AddOrUpdate(
                new Feed { CategoryId = 1, LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=sportal" },
                new Feed { CategoryId = 2, LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=scena" },
                new Feed { CategoryId = 3, LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=avtomoto" },
                new Feed { CategoryId = 4, LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=trendi" },
                new Feed { CategoryId = 5, LastUpdated = DateTime.Parse("2008-11-01T19:35:00.0000000Z"), NewsSiteId = 1, url = @"http://www.siol.net/rss.aspx?path=kultura" }
            );
        }
    }
}
