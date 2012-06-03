namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NewsFileCategoryadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("NewsFiles", "CategoryId", c => c.Int(nullable: false));
            AddForeignKey("NewsFiles", "CategoryId", "Categories", "CategoryId");
            CreateIndex("NewsFiles", "CategoryId");
        }
        
        public override void Down()
        {
            DropIndex("NewsFiles", new[] { "CategoryId" });
            DropForeignKey("NewsFiles", "CategoryId", "Categories");
            DropColumn("NewsFiles", "CategoryId");
        }
    }
}
