namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regin_Snapshot_Fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Area = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CreateVmTasks", "ErrorMessage", c => c.String());
            AddColumn("dbo.UserVms", "VurtualizationType", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "HyperVHostId", c => c.Guid(nullable: false));
            AddColumn("dbo.StandartVmTasks", "ErrorMessage", c => c.String());
            AddColumn("dbo.UpdateVmTasks", "ErrorMessage", c => c.String());
            CreateIndex("dbo.UserVms", "HyperVHostId");
            AddForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            DropColumn("dbo.UpdateVmTasks", "ErrorMessage");
            DropColumn("dbo.StandartVmTasks", "ErrorMessage");
            DropColumn("dbo.UserVms", "HyperVHostId");
            DropColumn("dbo.UserVms", "VurtualizationType");
            DropColumn("dbo.CreateVmTasks", "ErrorMessage");
            DropTable("dbo.Regions");
        }
    }
}
