namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemCenterVirtualManagerMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HyperVHostResources",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ResourceType = c.Int(nullable: false),
                        Value = c.String(),
                        Valid = c.Boolean(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        HyperVHostId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HyperVHosts", t => t.HyperVHostId, cascadeDelete: true)
                .Index(t => t.HyperVHostId);
            
            CreateTable(
                "dbo.HyperVHosts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Host = c.String(),
                        CoreNumber = c.Int(nullable: false),
                        RamSize = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Valid = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        SystemCenterVirtualManagerId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCenterVirtualManagers", t => t.SystemCenterVirtualManagerId, cascadeDelete: true)
                .Index(t => t.SystemCenterVirtualManagerId);
            
            CreateTable(
                "dbo.SystemCenterVirtualManagers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Host = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Synchronize = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HyperVHosts", "SystemCenterVirtualManagerId", "dbo.SystemCenterVirtualManagers");
            DropForeignKey("dbo.HyperVHostResources", "HyperVHostId", "dbo.HyperVHosts");
            DropIndex("dbo.HyperVHosts", new[] { "SystemCenterVirtualManagerId" });
            DropIndex("dbo.HyperVHostResources", new[] { "HyperVHostId" });
            DropTable("dbo.SystemCenterVirtualManagers");
            DropTable("dbo.HyperVHosts");
            DropTable("dbo.HyperVHostResources");
        }
    }
}
