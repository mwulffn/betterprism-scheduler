using FluentMigrator;

namespace Odk.Scheduler.Migrations.Migrations
{
    [Migration(12)]
    public class SessionIncident : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("fk_sessionincident_task").OnTable("scheduler_SessionIncidents");
            Delete.Table("scheduler_SessionIncidents");
        }

        public override void Up()
        {
            Create.Table("scheduler_SessionIncidents")
                .WithColumn("SessionIncidentId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("SessionId").AsGuid().NotNullable()
                .WithColumn("BPSessionId").AsGuid().NotNullable()
                .WithColumn("BPResourceId").AsGuid().NotNullable()
                .WithColumn("Resolution").AsInt32().WithDefaultValue(0)
                .WithColumn("Created").AsDateTime().NotNullable()
                .WithColumn("Closed").AsDateTime().Nullable();

            Create.ForeignKey("fk_sessionincident_task")
                .FromTable("scheduler_SessionIncidents").ForeignColumn("SessionId")
                .ToTable("scheduler_Sessions").PrimaryColumn("SessionId")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }
}