namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserCredentials : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Password", c => c.String(nullable: false));
            AddColumn("Users", "AccessToken", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Users", "AccessToken");
            DropColumn("Users", "Password");
        }
    }
}
