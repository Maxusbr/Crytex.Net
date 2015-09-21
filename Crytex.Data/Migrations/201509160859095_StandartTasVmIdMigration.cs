namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StandartTasVmIdMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms");
            DropIndex("dbo.StandartVmTasks", new[] { "VmId" });
            DropTable("dbo.StandartVmTasks");
            CreateTable(
                "dbo.StandartVmTasks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    VmId = c.Guid(nullable: false),
                    TaskType = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    StatusTask = c.Int(nullable: false),
                    UserId = c.String(),
                    Virtualization = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms");
            DropIndex("dbo.StandartVmTasks", new[] { "VmId" });
            DropTable("dbo.StandartVmTasks");
        }
    }
}
