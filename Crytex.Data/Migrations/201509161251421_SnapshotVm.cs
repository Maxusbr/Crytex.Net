namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnapshotVm : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SnapshotVms", "VmId", "dbo.UserVms");
            DropIndex("dbo.SnapshotVms", new[] { "VmId" });
        }
    }
}
