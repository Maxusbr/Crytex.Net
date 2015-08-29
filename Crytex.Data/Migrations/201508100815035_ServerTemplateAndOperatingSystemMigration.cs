namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServerTemplateAndOperatingSystemMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MinCoreCount = c.Int(nullable: false),
                        MinRamCount = c.Int(nullable: false),
                        MinHardDriveSize = c.Int(nullable: false),
                        ImageFileId = c.Int(nullable: false),
                        OperatingSystemId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDescriptors", t => t.ImageFileId, cascadeDelete: true)
                .ForeignKey("dbo.OperatingSystems", t => t.OperatingSystemId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ImageFileId)
                .Index(t => t.OperatingSystemId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OperatingSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageFileId = c.Int(nullable: false),
                        LoaderFileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDescriptors", t => t.ImageFileId, cascadeDelete: false)
                .ForeignKey("dbo.FileDescriptors", t => t.LoaderFileId, cascadeDelete: false)
                .Index(t => t.ImageFileId)
                .Index(t => t.LoaderFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServerTemplates", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServerTemplates", "OperatingSystemId", "dbo.OperatingSystems");
            DropForeignKey("dbo.OperatingSystems", "LoaderFileId", "dbo.FileDescriptors");
            DropForeignKey("dbo.OperatingSystems", "ImageFileId", "dbo.FileDescriptors");
            DropForeignKey("dbo.ServerTemplates", "ImageFileId", "dbo.FileDescriptors");
            DropIndex("dbo.OperatingSystems", new[] { "LoaderFileId" });
            DropIndex("dbo.OperatingSystems", new[] { "ImageFileId" });
            DropIndex("dbo.ServerTemplates", new[] { "UserId" });
            DropIndex("dbo.ServerTemplates", new[] { "OperatingSystemId" });
            DropIndex("dbo.ServerTemplates", new[] { "ImageFileId" });
            DropTable("dbo.OperatingSystems");
            DropTable("dbo.ServerTemplates");
        }
    }
}
