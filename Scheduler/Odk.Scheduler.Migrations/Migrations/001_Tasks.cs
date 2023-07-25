using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(1)]
    public class Tasks : Migration
    {
        public override void Down()
        {
            Delete.Table("scheduler_Tasks");
        }

        public override void Up()
        {
            Create.Table("scheduler_Tasks")
                .WithColumn("TaskId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable().WithDefaultValue("")
                .WithColumn("Trigger").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Enabled").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Cron").AsString().Nullable()
                .WithColumn("Workqueue").AsGuid().Nullable()
                .WithColumn("ScaleLimit").AsInt32().WithDefaultValue(1)
                .WithColumn("ScaleThreshold").AsInt32().WithDefaultValue(1)
                .WithColumn("Launch").AsGuid().Nullable()
                .WithColumn("Run").AsGuid().Nullable()
                .WithColumn("Complete").AsGuid().Nullable()
                .WithColumn("Fail").AsGuid().Nullable();
        }
    }
}