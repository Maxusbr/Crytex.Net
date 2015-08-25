namespace Project.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserVmCreateVmTaskMigration : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserVms");
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Date = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Level = c.String(),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.CreateVmTasks", "ServerTemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.CreateVmTasks", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserVms", "Guid", c => c.Guid(nullable: false, identity: true));
            AddColumn("dbo.UserVms", "CoreCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "RamCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "HardDriveSize", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "ServerTemplateId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UserVms", "Guid");
            CreateIndex("dbo.CreateVmTasks", "ServerTemplateId");
            CreateIndex("dbo.UserVms", "ServerTemplateId");
            AddForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserVms", "ServerTemplateId", "dbo.ServerTemplates", "Id", cascadeDelete: true);
            DropColumn("dbo.UserVms", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserVms", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.UserVms", "ServerTemplateId", "dbo.ServerTemplates");
            DropForeignKey("dbo.LogEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates");
            DropIndex("dbo.UserVms", new[] { "ServerTemplateId" });
            DropIndex("dbo.LogEntries", new[] { "UserId" });
            DropIndex("dbo.CreateVmTasks", new[] { "ServerTemplateId" });
            DropPrimaryKey("dbo.UserVms");
            DropColumn("dbo.UserVms", "ServerTemplateId");
            DropColumn("dbo.UserVms", "Status");
            DropColumn("dbo.UserVms", "HardDriveSize");
            DropColumn("dbo.UserVms", "RamCount");
            DropColumn("dbo.UserVms", "CoreCount");
            DropColumn("dbo.UserVms", "Guid");
            DropColumn("dbo.CreateVmTasks", "CreationDate");
            DropColumn("dbo.CreateVmTasks", "ServerTemplateId");
            DropTable("dbo.LogEntries");
            AddPrimaryKey("dbo.UserVms", "Id");
        }
    }
}
