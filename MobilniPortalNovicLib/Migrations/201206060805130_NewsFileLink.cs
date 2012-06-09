namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NewsFileLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("NewsFiles", "Link", c => c.String());
        }

        public override void Down()
        {
            DropColumn("NewsFiles", "Link");
        }
    }
}