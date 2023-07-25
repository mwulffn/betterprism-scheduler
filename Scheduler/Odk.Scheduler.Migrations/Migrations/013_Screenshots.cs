using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(13)]
    public class Screenshots : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("fk_screenshot_task").OnTable("scheduler_Screenshots");
            Delete.Table("scheduler_Screenshots");
        }

        public override void Up()
        {
            Create.Table("scheduler_Screenshots")
                .WithColumn("ScreenshotId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("SessionId").AsGuid().NotNullable()
                .WithColumn("BPSessionId").AsGuid().NotNullable()
                .WithColumn("Mimetype").AsString().NotNullable()
                .WithColumn("Screenshot").AsBinary(int.MaxValue).NotNullable()
                .WithColumn("Created").AsDateTime().NotNullable();

            Create.ForeignKey("fk_screenshot_task")
                .FromTable("scheduler_Screenshots").ForeignColumn("SessionId")
                .ToTable("scheduler_Sessions").PrimaryColumn("SessionId")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }
}