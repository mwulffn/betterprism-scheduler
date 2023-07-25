using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(10)]
    public class SessionDates : Migration
    {
        public override void Down()
        {
            Delete.Column("Created").FromTable("scheduler_Tasks");
            Delete.Column("Dispatched").FromTable("scheduler_Tasks");
            Delete.Column("Closed").FromTable("scheduler_Tasks");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Sessions").AddColumn("Created").AsDateTime().Nullable();
            Alter.Table("scheduler_Sessions").AddColumn("Dispatched").AsDateTime().Nullable();
            Alter.Table("scheduler_Sessions").AddColumn("Closed").AsDateTime().Nullable();
        }
    }
}