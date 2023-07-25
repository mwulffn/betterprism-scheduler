using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(15)]
    public class SoftDelete : Migration
    {
        public override void Down()
        {
            Delete.Column("Deleted").FromTable("scheduler_Tasks");
            Delete.Column("Deleted").FromTable("scheduler_Workblocks");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Tasks").AddColumn("Deleted").AsBoolean().WithDefaultValue(false);
            Alter.Table("scheduler_Workblocks").AddColumn("Deleted").AsBoolean().WithDefaultValue(false);
        }
    }
}