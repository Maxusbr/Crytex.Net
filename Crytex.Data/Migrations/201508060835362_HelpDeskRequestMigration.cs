namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpDeskRequestMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HelpDeskRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summary = c.String(),
                        Details = c.String(),
                        Status = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HelpDeskRequests", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.HelpDeskRequests", new[] { "UserId" });
            DropTable("dbo.HelpDeskRequests");
        }
    }
}
