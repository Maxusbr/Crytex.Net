namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StandartTasVmIdMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StandartVmTasks", "VmId", c => c.Guid(nullable: false));
            CreateIndex("dbo.StandartVmTasks", "VmId");
            AddForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms");
            DropIndex("dbo.StandartVmTasks", new[] { "VmId" });
            AlterColumn("dbo.StandartVmTasks", "VmId", c => c.Int(nullable: false));
        }
    }
}
