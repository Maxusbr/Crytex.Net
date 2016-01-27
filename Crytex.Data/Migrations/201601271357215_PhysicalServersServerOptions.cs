namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhysicalServersServerOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvailableOptionsPhysicalServers",
                c => new
                    {
                        PhysicalServerId = c.Guid(nullable: false),
                        OptionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PhysicalServerId, t.OptionId })
                .ForeignKey("dbo.ServerOptions", t => t.OptionId, cascadeDelete: true)
                .ForeignKey("dbo.PhysicalServers", t => t.PhysicalServerId, cascadeDelete: true)
                .Index(t => t.PhysicalServerId)
                .Index(t => t.OptionId);
            
            CreateTable(
                "dbo.ServerOptions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhysicalServers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProcessorName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OptionsPhysicalServers",
                c => new
                    {
                        PhysicalServerId = c.Guid(nullable: false),
                        OptionId = c.Guid(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.PhysicalServerId, t.OptionId })
                .ForeignKey("dbo.ServerOptions", t => t.OptionId, cascadeDelete: true)
                .ForeignKey("dbo.PhysicalServers", t => t.PhysicalServerId, cascadeDelete: true)
                .Index(t => t.PhysicalServerId)
                .Index(t => t.OptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionsPhysicalServers", "PhysicalServerId", "dbo.PhysicalServers");
            DropForeignKey("dbo.OptionsPhysicalServers", "OptionId", "dbo.ServerOptions");
            DropForeignKey("dbo.AvailableOptionsPhysicalServers", "PhysicalServerId", "dbo.PhysicalServers");
            DropForeignKey("dbo.AvailableOptionsPhysicalServers", "OptionId", "dbo.ServerOptions");
            DropIndex("dbo.OptionsPhysicalServers", new[] { "OptionId" });
            DropIndex("dbo.OptionsPhysicalServers", new[] { "PhysicalServerId" });
            DropIndex("dbo.AvailableOptionsPhysicalServers", new[] { "OptionId" });
            DropIndex("dbo.AvailableOptionsPhysicalServers", new[] { "PhysicalServerId" });
            DropTable("dbo.OptionsPhysicalServers");
            DropTable("dbo.PhysicalServers");
            DropTable("dbo.ServerOptions");
            DropTable("dbo.AvailableOptionsPhysicalServers");
        }
    }
}
