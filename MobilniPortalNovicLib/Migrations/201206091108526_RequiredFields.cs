namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Feeds", "url", c => c.String(nullable: false));
            AlterColumn("NewsFiles", "ShortContent", c => c.String(nullable: false));
            AlterColumn("NewsFiles", "Title", c => c.String(nullable: false));
            AlterColumn("NewsFiles", "Content", c => c.String(nullable: false));
            AlterColumn("NewsFiles", "Link", c => c.String(nullable: false));
            AlterColumn("Users", "Username", c => c.String(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("Users", "Username", c => c.String());
            AlterColumn("NewsFiles", "Link", c => c.String());
            AlterColumn("NewsFiles", "Content", c => c.String());
            AlterColumn("NewsFiles", "Title", c => c.String());
            AlterColumn("NewsFiles", "ShortContent", c => c.String());
            AlterColumn("Feeds", "url", c => c.String());
        }
    }
}