namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class First : DbMigration
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
                        url = c.String(),
                        CategoryId = c.Int(nullable: false),
                        NewsSiteId = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FeedId)
                .ForeignKey("Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("NewsSites", t => t.NewsSiteId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.NewsSiteId);

            CreateTable(
                "Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);

            CreateTable(
                "NewsFiles",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        ShortContent = c.String(),
                        Title = c.String(),
                        Content = c.String(),
                        PubDate = c.DateTime(nullable: false),
                        FeedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("Feeds", t => t.FeedId, cascadeDelete: true)
                .Index(t => t.FeedId)
                .Index(x => x.PubDate);

            CreateTable(
                "ClickCounters",
                c => new
                    {
                        ClickId = c.Int(nullable: false, identity: true),
                        ClickDate = c.DateTime(nullable: false),
                        Location = c.String(),
                        NewsId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClickId)
                .ForeignKey("NewsFiles", t => t.NewsId, cascadeDelete: true)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.NewsId)
                .Index(t => t.UserId);

            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
        }

        public override void Down()
        {
            DropIndex("ClickCounters", new[] { "UserId" });
            DropIndex("ClickCounters", new[] { "NewsId" });
            DropIndex("NewsFiles", new[] { "FeedId" });
            DropIndex("Feeds", new[] { "NewsSiteId" });
            DropIndex("Feeds", new[] { "CategoryId" });
            DropForeignKey("ClickCounters", "UserId", "Users");
            DropForeignKey("ClickCounters", "NewsId", "NewsFiles");
            DropForeignKey("NewsFiles", "FeedId", "Feeds");
            DropForeignKey("Feeds", "NewsSiteId", "NewsSites");
            DropForeignKey("Feeds", "CategoryId", "Categories");
            DropTable("Users");
            DropTable("ClickCounters");
            DropTable("NewsFiles");
            DropTable("Categories");
            DropTable("Feeds");
            DropTable("NewsSites");
        }
    }
}