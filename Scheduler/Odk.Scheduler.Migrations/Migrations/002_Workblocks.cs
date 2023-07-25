using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(2)]
    public class Workblocks : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("ix_TasksWorkblock_fail").OnTable("scheduler_Tasks");
            Delete.ForeignKey("ix_TasksWorkblock_complete").OnTable("scheduler_Tasks");
            Delete.ForeignKey("ix_TasksWorkblock_run").OnTable("scheduler_Tasks");
            Delete.ForeignKey("ix_TasksWorkblock_launch").OnTable("scheduler_Tasks");
            Delete.Table("scheduler_Workblocks");
        }

        public override void Up()
        {
            Create.Table("scheduler_Workblocks")
                .WithColumn("ProcessId").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("PostCompletionDelay").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Intention").AsInt32().NotNullable()
                .WithColumn("Parameters").AsString();

            Create.ForeignKey("ix_TasksWorkblock_launch")
                .FromTable("scheduler_Tasks").ForeignColumn("Launch")
                .ToTable("scheduler_Workblocks").PrimaryColumn("ProcessId")
                .OnDeleteOrUpdate(System.Data.Rule.None);

            Create.ForeignKey("ix_TasksWorkblock_run")
                .FromTable("scheduler_Tasks").ForeignColumn("Run")
                .ToTable("scheduler_Workblocks").PrimaryColumn("ProcessId")
                .OnDeleteOrUpdate(System.Data.Rule.None);

            Create.ForeignKey("ix_TasksWorkblock_complete")
                .FromTable("scheduler_Tasks").ForeignColumn("Complete")
                .ToTable("scheduler_Workblocks").PrimaryColumn("ProcessId")
                .OnDeleteOrUpdate(System.Data.Rule.None);

            Create.ForeignKey("ix_TasksWorkblock_fail")
                .FromTable("scheduler_Tasks").ForeignColumn("Fail")
                .ToTable("scheduler_Workblocks").PrimaryColumn("ProcessId")
                .OnDeleteOrUpdate(System.Data.Rule.None);
        }
    }
}