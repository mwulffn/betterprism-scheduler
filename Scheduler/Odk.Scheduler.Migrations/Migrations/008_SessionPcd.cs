using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(8)]
    public class SessionPcds : Migration
    {
        public override void Down()
        {
            Delete.Column("LaunchPcd").FromTable("scheduler_Sessions");
            Delete.Column("RunPcd").FromTable("scheduler_Sessions");
            Delete.Column("CompletePcd").FromTable("scheduler_Sessions");
            Delete.Column("FailPcd").FromTable("scheduler_Sessions");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Sessions").AddColumn("LaunchPcd").AsInt32().WithDefaultValue(0);
            Alter.Table("scheduler_Sessions").AddColumn("RunPcd").AsInt32().WithDefaultValue(0);
            Alter.Table("scheduler_Sessions").AddColumn("CompletePcd").AsInt32().WithDefaultValue(0);
            Alter.Table("scheduler_Sessions").AddColumn("FailPcd").AsInt32().WithDefaultValue(0);
        }
    }
}