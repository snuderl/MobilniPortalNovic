namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FeedCategoryRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Feeds", "CategoryId", "Categories");
            DropIndex("Feeds", new[] { "CategoryId" });
            AddColumn("Feeds", "FeedName", c => c.String());
            DropColumn("Feeds", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("Feeds", "CategoryId", c => c.Int(nullable: false));
            DropColumn("Feeds", "FeedName");
            CreateIndex("Feeds", "CategoryId");
            AddForeignKey("Feeds", "CategoryId", "Categories", "CategoryId", cascadeDelete: true);
        }
    }
}
