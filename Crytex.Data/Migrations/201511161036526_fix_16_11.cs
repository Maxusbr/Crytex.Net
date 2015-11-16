namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class fix_16_11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates");
            DropForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.UpdateVmTasks", "VmId", "dbo.UserVms");
            DropIndex("dbo.CreateVmTasks", new[] { "ServerTemplateId" });
            DropIndex("dbo.StandartVmTasks", new[] { "VmId" });
            DropIndex("dbo.UpdateVmTasks", new[] { "VmId" });
            CreateTable(
                "dbo.Statistics",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Value = c.Single(nullable: false),
                    Type = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.OperatingSystems", "Family", c => c.Int(nullable: false));

            CreateTable(
          "dbo.SnapshotVms",
          c => new
          {
              Id = c.Int(nullable: false, identity: true),
              Name = c.String(),
              Date = c.DateTime(nullable: false),
              VmId = c.Guid(nullable: false),
              Validation = c.Boolean(nullable: false),
          })
          .PrimaryKey(t => t.Id);

            CreateIndex("dbo.SnapshotVms", "VmId");
            AddForeignKey("dbo.SnapshotVms", "VmId", "dbo.UserVms", "Id", cascadeDelete: true);


            DropTable("dbo.CreateVmTasks");
            DropTable("dbo.StandartVmTasks");
            DropTable("dbo.UpdateVmTasks");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.UpdateVmTasks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    VmId = c.Guid(nullable: false),
                    Cpu = c.Int(nullable: false),
                    Ram = c.Int(nullable: false),
                    Hdd = c.Int(nullable: false),
                    Name = c.String(),
                    StatusTask = c.Int(nullable: false),
                    UserId = c.String(),
                    Virtualization = c.Int(nullable: false),
                    ErrorMessage = c.String(),
                })
                .PrimaryKey(t => t.Id);

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
                    ErrorMessage = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CreateVmTasks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ServerTemplateId = c.Int(nullable: false),
                    CreationDate = c.DateTime(nullable: false),
                    Cpu = c.Int(nullable: false),
                    Ram = c.Int(nullable: false),
                    Hdd = c.Int(nullable: false),
                    Name = c.String(),
                    StatusTask = c.Int(nullable: false),
                    UserId = c.String(),
                    Virtualization = c.Int(nullable: false),
                    ErrorMessage = c.String(),
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.OperatingSystems", "Family");
            DropTable("dbo.Statistics");
            CreateIndex("dbo.UpdateVmTasks", "VmId");
            CreateIndex("dbo.StandartVmTasks", "VmId");
            CreateIndex("dbo.CreateVmTasks", "ServerTemplateId");
            AddForeignKey("dbo.UpdateVmTasks", "VmId", "dbo.UserVms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StandartVmTasks", "VmId", "dbo.UserVms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CreateVmTasks", "ServerTemplateId", "dbo.ServerTemplates", "Id", cascadeDelete: true);

            DropForeignKey("dbo.SnapshotVms", "VmId", "dbo.UserVms");
            DropIndex("dbo.SnapshotVms", new[] { "VmId" });
            DropTable("dbo.SnapshotVms");
        }
    }
}
