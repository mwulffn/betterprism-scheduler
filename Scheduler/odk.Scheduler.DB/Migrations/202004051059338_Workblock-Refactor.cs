namespace odk.Scheduler.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class WorkblockRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "AllocatedResource", c => c.Guid(nullable: true));
            AddColumn("dbo.TaskWorkblocks", "Intention", c => c.Int(nullable: false, defaultValue: 0));
            /*           DropColumn("dbo.TaskWorkblocks", "Intention");
                       AddColumn("dbo.Workblocks", "Intention", c => c.Int(nullable: false, defaultValue: 0));
                       AddColumn("dbo.Workblocks", "DefaultParameters", c => c.String(nullable: true));
            */
        }

        public override void Down()
        {
            DropColumn("dbo.TaskWorkblocks", "Intention");
            DropColumn("dbo.Sessions", "AllocatedResource");
            /*         AddColumn("dbo.TaskWorkblocks", "Intention", c => c.Int(nullable: false, defaultValue: 0));
                     DropColumn("dbo.Workblocks", "Intention");
                     DropColumn("dbo.Workblocks", "Parameters");
              */
        }
    }
}
