using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(3)]
    public class _003_AtDates : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("fk_atdates").OnTable("scheduler_AtDates");
            Delete.Table("scheduler_AtDates");
        }

        public override void Up()
        {
            Create.Table("scheduler_AtDates")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("AtDate").AsDateTime().NotNullable()
                .WithColumn("TaskId").AsGuid().NotNullable();

            Create.ForeignKey("fk_atdates")
                .FromTable("scheduler_AtDates").ForeignColumn("TaskId")
                .ToTable("scheduler_Tasks").PrimaryColumn("TaskId")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }
}