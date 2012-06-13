namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ClicksGotTimeOfDayAndDayOfWeekField : DbMigration
    {
        public override void Up()
        {
            AddColumn("ClickCounters", "DayOfWeek", c => c.Int(nullable: false));
            AddColumn("ClickCounters", "TimeOfDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ClickCounters", "TimeOfDay");
            DropColumn("ClickCounters", "DayOfWeek");
        }
    }
}
