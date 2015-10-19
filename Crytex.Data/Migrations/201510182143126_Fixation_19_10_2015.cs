namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixation_19_10_2015 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperatingSystems", "LoaderFileId", "dbo.FileDescriptors");
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropIndex("dbo.OperatingSystems", new[] { "LoaderFileId" });
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            CreateTable(
                "dbo.OAuthClientApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        EnumApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OAuthRefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VmWareVCenters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        ServerAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StateMachines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CpuLoad = c.Int(nullable: false),
                        RamLoad = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        VmId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskV2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResourceId = c.Guid(),
                        TypeTask = c.Int(nullable: false),
                        StatusTask = c.Int(nullable: false),
                        ResourceType = c.Int(nullable: false),
                        Options = c.String(),
                        UserId = c.String(),
                        ErrorMessage = c.String(),
                        Virtualization = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        StartedAt = c.DateTime(),
                        CompletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.HelpDeskRequests", "Read", c => c.Boolean(nullable: false));
            AddColumn("dbo.OperatingSystems", "ServerTemplateName", c => c.String());
            AddColumn("dbo.UserVms", "VmWareCenterId", c => c.Guid());
            AlterColumn("dbo.UserVms", "HyperVHostId", c => c.Guid());
            CreateIndex("dbo.UserVms", "HyperVHostId");
            CreateIndex("dbo.UserVms", "VmWareCenterId");
            AddForeignKey("dbo.UserVms", "VmWareCenterId", "dbo.VmWareVCenters", "Id");
            AddForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts", "Id");
            DropColumn("dbo.OperatingSystems", "LoaderFileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OperatingSystems", "LoaderFileId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropForeignKey("dbo.UserVms", "VmWareCenterId", "dbo.VmWareVCenters");
            DropIndex("dbo.UserVms", new[] { "VmWareCenterId" });
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            AlterColumn("dbo.UserVms", "HyperVHostId", c => c.Guid(nullable: false));
            DropColumn("dbo.UserVms", "VmWareCenterId");
            DropColumn("dbo.OperatingSystems", "ServerTemplateName");
            DropColumn("dbo.HelpDeskRequests", "Read");
            DropTable("dbo.TaskV2");
            DropTable("dbo.StateMachines");
            DropTable("dbo.VmWareVCenters");
            DropTable("dbo.OAuthRefreshTokens");
            DropTable("dbo.OAuthClientApplications");
            CreateIndex("dbo.UserVms", "HyperVHostId");
            CreateIndex("dbo.OperatingSystems", "LoaderFileId");
            AddForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OperatingSystems", "LoaderFileId", "dbo.FileDescriptors", "Id", cascadeDelete: true);
        }
    }
}
