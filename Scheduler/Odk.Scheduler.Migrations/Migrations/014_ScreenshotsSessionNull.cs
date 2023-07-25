using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(14)]
    public class ScreenshotsSessionNull : Migration
    {
        public override void Down()
        {
            Alter.Table("scheduler_Screenshots").AlterColumn("SessionId").AsGuid().NotNullable();
        }

        public override void Up()
        {
            Alter.Table("scheduler_Screenshots").AlterColumn("SessionId").AsGuid().Nullable();
        }
    }
}