namespace odk.Scheduler.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResourceUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "AllocatedResource", c => c.Guid(nullable: true));
            AddColumn("dbo.TaskWorkblocks", "Intention", c => c.Int(nullable: false,defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskWorkblocks", "Intention");
            DropColumn("dbo.Sessions", "AllocatedResource");
        }
    }
}
