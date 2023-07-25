using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(5)]
    public class WorkblockKey : Migration
    {
        public override void Down()
        {
            Delete.Column("ProcessId").FromTable("scheduler_Workblocks");
            Rename.Column("WorkblockId").OnTable("scheduler_Workblocks").To("ProcessId");
        }

        public override void Up()
        {
            Rename.Column("ProcessId").OnTable("scheduler_Workblocks").To("WorkblockId");
            Alter.Table("scheduler_Workblocks").AddColumn("ProcessId").AsGuid().NotNullable();
        }
    }
}