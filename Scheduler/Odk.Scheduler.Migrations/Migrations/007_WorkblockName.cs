using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(7)]
    public class WorkblockNameParameters : Migration
    {
        public override void Down()
        {
            Delete.Column("Name").FromTable("scheduler_Workblocks");
        }

        public override void Up()
        {
            Alter.Table("scheduler_Workblocks").AddColumn("Name").AsString().Nullable();
        }
    }
}