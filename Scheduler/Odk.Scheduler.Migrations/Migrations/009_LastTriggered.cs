using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(9)]
    public class LastTriggered : Migration
    {
        public override void Down()
        {
            Delete.Column("LastTriggered").FromTable("scheduler_Tasks");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Tasks").AddColumn("LastTriggered").AsDateTime().Nullable();
        }
    }
}