namespace Crytex.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserVmMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateVmTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cpu = c.Int(nullable: false),
                        Ram = c.Int(nullable: false),
                        Hdd = c.Int(nullable: false),
                        Name = c.String(),
                        StatusTask = c.Int(nullable: false),
                        UserId = c.String(),
                        Virtualization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StandartVmTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VmId = c.Int(nullable: false),
                        StatusTask = c.Int(nullable: false),
                        UserId = c.String(),
                        Virtualization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UpdateVmTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VmId = c.Int(nullable: false),
                        Cpu = c.Int(nullable: false),
                        Ram = c.Int(nullable: false),
                        Hdd = c.Int(nullable: false),
                        Name = c.String(),
                        StatusTask = c.Int(nullable: false),
                        UserId = c.String(),
                        Virtualization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserVms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserVms");
            DropTable("dbo.UpdateVmTasks");
            DropTable("dbo.StandartVmTasks");
            DropTable("dbo.Messages");
            DropTable("dbo.CreateVmTasks");
        }
    }
}
