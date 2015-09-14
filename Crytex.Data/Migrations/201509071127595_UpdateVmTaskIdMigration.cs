namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVmTaskIdMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UpdateVmTasks", "VmId");
            AddColumn("dbo.UpdateVmTasks", "VmId", c => c.Guid(nullable: false));
            CreateIndex("dbo.UpdateVmTasks", "VmId");
            AddForeignKey("dbo.UpdateVmTasks", "VmId", "dbo.UserVms", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UpdateVmTasks", "VmId", "dbo.UserVms");
            DropIndex("dbo.UpdateVmTasks", new[] { "VmId" });
            DropColumn("dbo.UpdateVmTasks", "VmId");
            AddColumn("dbo.UpdateVmTasks", "VmId", c => c.Int(nullable: false));
           
        }
    }
}
