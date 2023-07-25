namespace odk.Scheduler.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AtDates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RunAt = c.DateTime(nullable: false),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Trigger = c.Int(nullable: false),
                        Cron = c.String(),
                        Workqueue = c.String(),
                        ScaleLimit = c.Int(nullable: false),
                        ScaleThreshold = c.Int(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TaskState = c.Int(nullable: false),
                        BPSessionId = c.Guid(nullable: false),
                        DelayUntil = c.DateTime(nullable: false),
                        Master = c.Boolean(nullable: false),
                        AllocatedResource = c.Guid(),
                        Updated = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        StopRequested = c.Boolean(nullable: false),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
            CreateTable(
                "dbo.TaskWorkblocks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Parameters = c.String(),
                        Task_Id = c.Guid(),
                        Workblock_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .ForeignKey("dbo.Workblocks", t => t.Workblock_Id)
                .Index(t => t.Task_Id)
                .Index(t => t.Workblock_Id);
            
            CreateTable(
                "dbo.Workblocks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProcessId = c.Guid(nullable: false),
                        Name = c.String(),
                        Intention = c.Int(nullable: false),
                        DefaultParameters = c.String(),
                        PostCompletionDelay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskWorkblocks", "Workblock_Id", "dbo.Workblocks");
            DropForeignKey("dbo.TaskWorkblocks", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Sessions", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.AtDates", "Task_Id", "dbo.Tasks");
            DropIndex("dbo.TaskWorkblocks", new[] { "Workblock_Id" });
            DropIndex("dbo.TaskWorkblocks", new[] { "Task_Id" });
            DropIndex("dbo.Sessions", new[] { "Task_Id" });
            DropIndex("dbo.AtDates", new[] { "Task_Id" });
            DropTable("dbo.Workblocks");
            DropTable("dbo.TaskWorkblocks");
            DropTable("dbo.Sessions");
            DropTable("dbo.Tasks");
            DropTable("dbo.AtDates");
            DropTable("dbo.ActivityLogs");
        }
    }
}
