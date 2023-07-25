using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(16)]
    public class ScreenshotsItemKey : Migration
    {
        public override void Down()
        {
            Delete.Column("ItemKey").FromTable("scheduler_Screenshots");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Screenshots").AddColumn("ItemKey").AsString().WithDefaultValue("");
        }
    }
}