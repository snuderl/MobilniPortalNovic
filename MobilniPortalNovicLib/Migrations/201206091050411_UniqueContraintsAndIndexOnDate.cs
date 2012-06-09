namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UniqueContraintsAndIndexOnDate : DbMigration
    {
        public override void Up()
        {
            CreateIndex("NewsFiles", "PubDate", false, "Index_PubDate");
        }

        public override void Down()
        {
            DropIndex("NewsFiles", "Index_PubDate");
        }
    }
}
