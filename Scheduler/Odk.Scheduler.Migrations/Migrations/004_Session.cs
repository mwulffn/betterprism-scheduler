using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(4)]
    public class Session : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("fk_session_task").OnTable("scheduler_Sessions");
            Delete.Table("scheduler_Sessions");
        }

        public override void Up()
        {
            Create.Table("scheduler_Sessions")
                .WithColumn("SessionId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("TaskId").AsGuid().Nullable()
                .WithColumn("State").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("DelayStateTransition").AsDateTime().NotNullable()
                .WithColumn("BPSessionId").AsGuid().Nullable()
                .WithColumn("AllocatedResource").AsGuid().Nullable()
                .WithColumn("Master").AsBoolean().NotNullable()
                .WithColumn("StopRequested").AsBoolean().NotNullable()
                .WithColumn("Launch").AsGuid().Nullable()
                .WithColumn("Run").AsGuid().Nullable()
                .WithColumn("Complete").AsGuid().Nullable()
                .WithColumn("Fail").AsGuid().Nullable();

            Create.ForeignKey("fk_session_task")
                .FromTable("scheduler_Sessions").ForeignColumn("TaskId")
                .ToTable("scheduler_Tasks").PrimaryColumn("TaskId")
                .OnDeleteOrUpdate(System.Data.Rule.SetNull);
        }
    }
}