namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CategoryParentsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("Categories", "ParentCategoryId", c => c.Int());
            AddColumn("Categories", "ParentCategory_CategoryId", c => c.Int());
            AddForeignKey("Categories", "ParentCategory_CategoryId", "Categories", "CategoryId");
            CreateIndex("Categories", "ParentCategory_CategoryId");
        }
        
        public override void Down()
        {
            DropIndex("Categories", new[] { "ParentCategory_CategoryId" });
            DropForeignKey("Categories", "ParentCategory_CategoryId", "Categories");
            DropColumn("Categories", "ParentCategory_CategoryId");
            DropColumn("Categories", "ParentCategoryId");
        }
    }
}
