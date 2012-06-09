namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("Categories", "ParentCategoryId", c => c.Int());
            AddForeignKey("Categories", "ParentCategoryId", "Categories", "CategoryId");
            CreateIndex("Categories", "ParentCategoryId");
        }

        public override void Down()
        {
            DropIndex("Categories", new[] { "ParentCategoryId" });
            DropForeignKey("Categories", "ParentCategoryId", "Categories");
            DropColumn("Categories", "ParentCategoryId");
        }
    }
}