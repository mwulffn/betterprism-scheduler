using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(11)]
    public class HasBeenTriggered : Migration
    {
        public override void Down()
        {
            Delete.Column("HasBeenTriggered").FromTable("scheduler_AtDates");
        }

        public override void Up()
        {
            Alter.Table("scheduler_AtDates").AddColumn("HasBeenTriggered").AsBoolean().WithDefaultValue(false);
        }
    }
}