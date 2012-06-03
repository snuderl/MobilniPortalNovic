namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ClickCounterCategoryadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("ClickCounters", "CategoryId", c => c.Int(nullable: false));
            AddForeignKey("ClickCounters", "CategoryId", "Categories", "CategoryId");
            CreateIndex("ClickCounters", "CategoryId");
        }
        
        public override void Down()
        {
            DropIndex("ClickCounters", new[] { "CategoryId" });
            DropForeignKey("ClickCounters", "CategoryId", "Categories");
            DropColumn("ClickCounters", "CategoryId");
        }
    }
}
