namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserVmAndCreateVmTask : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserVms");
            CreateTable(
                "dbo.UserVms",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true)
                })
                .PrimaryKey(t => t.Id);
            AddColumn("dbo.CreateVmTasks", "ServerTemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.CreateVmTasks", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserVms", "CoreCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "RamCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "HardDriveSize", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "ServerTemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.UserVms", "Name", c => c.String());
            CreateIndex("dbo.CreateVmTasks", "ServerTemplateId");
            CreateIndex("dbo.UserVms", "ServerTemplateId");
            AddForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserVms", "ServerTemplateId", "dbo.ServerTemplates", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVms", "ServerTemplateId", "dbo.ServerTemplates");
            DropForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates");
            DropIndex("dbo.UserVms", new[] { "ServerTemplateId" });
            DropIndex("dbo.CreateVmTasks", new[] { "ServerTemplateId" });
            DropPrimaryKey("dbo.UserVms");
            AlterColumn("dbo.UserVms", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.LogEntries", "Date", c => c.String());
            AlterColumn("dbo.EmailInfoes", "DateSending", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserVms", "Name");
            DropColumn("dbo.UserVms", "ServerTemplateId");
            DropColumn("dbo.UserVms", "Status");
            DropColumn("dbo.UserVms", "HardDriveSize");
            DropColumn("dbo.UserVms", "RamCount");
            DropColumn("dbo.UserVms", "CoreCount");
            DropColumn("dbo.CreateVmTasks", "CreationDate");
            DropColumn("dbo.CreateVmTasks", "ServerTemplateId");
            AddPrimaryKey("dbo.UserVms", "Id");
        }
    }
}
