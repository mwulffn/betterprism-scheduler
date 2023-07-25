using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(6)]
    public class SessionParameters : Migration
    {
        public override void Down()
        {
            Delete.Column("LaunchParameters").FromTable("scheduler_Sessions");
            Delete.Column("RunParameters").FromTable("scheduler_Sessions");
            Delete.Column("CompleteParameters").FromTable("scheduler_Sessions");
            Delete.Column("FailParameters").FromTable("scheduler_Sessions");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Sessions").AddColumn("LaunchParameters").AsString().Nullable();
            Alter.Table("scheduler_Sessions").AddColumn("RunParameters").AsString().Nullable();
            Alter.Table("scheduler_Sessions").AddColumn("CompleteParameters").AsString().Nullable();
            Alter.Table("scheduler_Sessions").AddColumn("FailParameters").AsString().Nullable();
        }
    }
}