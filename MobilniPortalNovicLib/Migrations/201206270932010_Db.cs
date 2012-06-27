namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "NewsSites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SiteId);
            
            CreateTable(
                "Feeds",
                c => new
                    {
                        FeedId = c.Int(nullable: false, identity: true),
                        url = c.String(nullable: false),
                        FeedName = c.String(),
                        NewsSiteId = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FeedId)
                .ForeignKey("NewsSites", t => t.NewsSiteId, cascadeDelete: true)
                .Index(t => t.NewsSiteId);
            
            CreateTable(
                "Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("Categories", t => t.ParentCategoryId)
                .Index(t => t.ParentCategoryId);
            
            CreateTable(
                "NewsFiles",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        ShortContent = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PubDate = c.DateTime(nullable: false),
                        FeedId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Link = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("Feeds", t => t.FeedId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.FeedId);
            
            CreateTable(
                "ClickCounters",
                c => new
                    {
                        ClickId = c.Int(nullable: false, identity: true),
                        ClickDate = c.DateTime(nullable: false),
                        Longitude = c.Double(),
                        Latitude = c.Double(),
                        NewsId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        TimeOfDay = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClickId)
                .ForeignKey("NewsFiles", t => t.NewsId, cascadeDelete: true)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.NewsId)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        AccessToken = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("ClickCounters", new[] { "CategoryId" });
            DropIndex("ClickCounters", new[] { "UserId" });
            DropIndex("ClickCounters", new[] { "NewsId" });
            DropIndex("NewsFiles", new[] { "FeedId" });
            DropIndex("NewsFiles", new[] { "CategoryId" });
            DropIndex("Categories", new[] { "ParentCategoryId" });
            DropIndex("Feeds", new[] { "NewsSiteId" });
            DropForeignKey("ClickCounters", "CategoryId", "Categories");
            DropForeignKey("ClickCounters", "UserId", "Users");
            DropForeignKey("ClickCounters", "NewsId", "NewsFiles");
            DropForeignKey("NewsFiles", "FeedId", "Feeds");
            DropForeignKey("NewsFiles", "CategoryId", "Categories");
            DropForeignKey("Categories", "ParentCategoryId", "Categories");
            DropForeignKey("Feeds", "NewsSiteId", "NewsSites");
            DropTable("Users");
            DropTable("ClickCounters");
            DropTable("NewsFiles");
            DropTable("Categories");
            DropTable("Feeds");
            DropTable("NewsSites");
        }
    }
}
