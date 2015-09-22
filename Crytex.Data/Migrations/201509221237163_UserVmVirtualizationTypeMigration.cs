namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserVmVirtualizationTypeMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserVms", "VurtualizationType", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "HyperVHostId", c => c.Guid(nullable: true));
            CreateIndex("dbo.UserVms", "HyperVHostId");
            AddForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            DropColumn("dbo.UserVms", "HyperVHostId");
            DropColumn("dbo.UserVms", "VurtualizationType");
        }
    }
}
